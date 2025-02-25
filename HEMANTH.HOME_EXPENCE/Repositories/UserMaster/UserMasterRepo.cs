using HEMANTH.HOME_EXPENCE.Models.UserMaster.AddExpence;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.DownloadLastBill;
using HEMANTH.HOME_EXPENCE.Models.UserMaster.UserDashBoard;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.UserMaster;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family;
using HEMANTH.HOME_EXPENSE.ServiceInterface;
using IIITS.EFCore.Repositories;
using IIITS.LMS.Repositories.GeneralTables.ExpenseDetailsTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace HEMANTH.HOME_EXPENCE.Repositories.UserMaster
{
    public class UserMasterRepo : IUserMasterRepo
    {
        private readonly EmailService _emailService;

        private readonly IConfiguration _configuration;

        private readonly LMSMasterServiceDBContext _dbContext;

        public UserMasterRepo(LMSMasterServiceDBContext dbContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = dbContext; // DbContext should be injected, and DI will manage its lifecycle
            _configuration = configuration;
            _emailService = emailService;
        }




        public async Task<UserDashBoard> GetUserDetails(int UserId)
        {
            UserDashBoard dashBoard = new UserDashBoard();
            try
            {
                var result = await (from login in _dbContext.LoginDetails
                                    join family in _dbContext.FamilyTbl on login.UserFamilyId equals family.FamilyID into familyJoin
                                    from family in familyJoin.DefaultIfEmpty() // Left Join
                                    join expenceMaster in _dbContext.ExpMaster on login.UserId equals expenceMaster.ExpenceUserId into expenceMasterJoin
                                    from expenceMaster in expenceMasterJoin.DefaultIfEmpty() // Left Join
                                    join expenceDetails in _dbContext.ExpDetails on expenceMaster.ExpenceID equals expenceDetails.ExpenceDetailsExpID into expenceDetailsJoin
                                    from expenceDetails in expenceDetailsJoin.DefaultIfEmpty() // Left Join
                                    where login.UserId == UserId
                                    select new
                                    {
                                        LoginUserName = login.UserName,
                                        FamilyName = family != null ? family.FamilyName : "NA", 
                                        ExpencePrice = expenceDetails != null ? expenceDetails.ExpencePersPrice : 0, 
                                        FamilyID = family != null ? family.FamilyID : 0,
                                        isAdmin = login.AdminStatus
                                    }).ToListAsync();

                if (result.Any())
                {
                    var firstItem = result.FirstOrDefault();
                    dashBoard.UserName = firstItem.LoginUserName;
                    dashBoard.FamilyName = firstItem.FamilyName;
                    dashBoard.UserID = UserId;
                    dashBoard.AdminStatus = firstItem.isAdmin;
                    int fam =  _dbContext.LoginDetails.Where(x => x.UserId == UserId).FirstOrDefault().UserFamilyId;
                    dashBoard.FamilMembersCount = await _dbContext.LoginDetails.CountAsync(x => x.UserFamilyId == fam);

                    DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime toDate = DateTime.Now;

                    dashBoard.PersonalExpence = _dbContext.ExpMaster
                        .Where(x => x.ExpenceDate <= toDate && x.ExpenceDate >= fromDate && x.ExpenceUserId == UserId)
                        .Sum(x => x.ExpensePrice);

                    dashBoard.FamilyExpence = _dbContext.ExpMaster
                        .Where(x => x.ExpenceDate <= toDate && x.ExpenceDate >= fromDate && x.ExpenceFamId == fam)
                        .Sum(x => x.ExpensePrice);

                    var expenses = await _dbContext.ExpMaster
                        .ToListAsync(); 

                    var monthlyExpenses = expenses
                        .GroupBy(exp => new { exp.ExpenceDate.Year, exp.ExpenceDate.Month })
                        .Select(g => new MonthlyExpense
                        {
                            Month = $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)} {g.Key.Year}",
                            Personal = g.Where(exp => exp.ExpenceUserId == UserId).Sum(exp => exp.ExpensePrice),
                            Family = g.Where(exp => exp.ExpenceFamId == fam).Sum(exp => exp.ExpensePrice)
                        })
                        .Where(exp => exp.Personal > 0 || exp.Family > 0)
                        .OrderBy(exp => DateTime.Parse(exp.Month))
                        .ToList(); 

                    dashBoard.MonthlyExpenses = monthlyExpenses.ToList();




                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return dashBoard;
        }


        public async Task<IEnumerable<Expense>> GetExpenceDetails(int UserId)
        {
            try
            {
                
                var familyId = await _dbContext.LoginDetails
    .Where(x => x.UserId == UserId  )
    .Select(x => x.UserFamilyId)
    .FirstOrDefaultAsync();
                var expDetails = await (
            from expMaster in _dbContext.ExpMaster
            join categ in _dbContext.CategoryTable 
                on expMaster.ExpenceCategory equals categ.CategoryId 
            
            join family in _dbContext.FamilyTbl 
                on expMaster.ExpenceFamId equals family.FamilyID
            
            join user in _dbContext.LoginDetails 
                on expMaster.ExpenceUserId equals user.UserId 

            where family.FamilyID == familyId
            select new Expense
            {
                Category = categ != null ? categ.CatagoryName : "No Category",
                Description = categ != null ? categ.CatergoryDescription : "No Description",
                Amount = expMaster.ExpensePrice,
                Date = expMaster.ExpenceDate,
                UserName = user != null ? user.UserName : "No User",
                ExpenseMasterId= expMaster.ExpenceID,
                UserId= user.UserId
            }
        ).ToListAsync();


                var distinctExpDetails = expDetails
       .GroupBy(exp => new { exp.Category, exp.Description, exp.Amount, exp.Date, exp.UserName })
       .OrderByDescending(x => x.Key.Date) 
       .Select(group => group.First())
       .ToList();

                return distinctExpDetails;


            }
            catch (Exception ex)
            {
                return new List< Expense>();            }

        }


        public async Task<IEnumerable<SelectListItem>> GetCategoryDetails()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                var catdetails = await _dbContext.CategoryTable.ToListAsync();

                foreach (var category in catdetails)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = category.CatagoryName,
                        Value = category.CategoryId.ToString()
                    });
                }

                lst.Insert(0, new SelectListItem { Text = "Select", Value = "0" });
            }
            catch (Exception ex)
            {
                lst.Add(new SelectListItem { Text = "Select", Value = "0" });
            }
            return lst;
        }

        public async Task<IEnumerable<User>> GetUserDetailsForAddExence(int userId)
        {
            List<User> lst = new List<User>();
            try
            {
                var familyId = await _dbContext.LoginDetails
    .Where(x => x.UserId == userId)
    .Select(x => x.UserFamilyId)
    .FirstOrDefaultAsync();

                var userDetails = await _dbContext.LoginDetails.Where(x => x.UserFamilyId == familyId && x.UserActiveStatus==1).ToListAsync();
                     foreach(var user in userDetails)
                {
                    lst.Add(new User
                    {
                        Id = user.UserId,
                        Name = user.UserName,
                        Amount = 0
                    });
                  
                }
                               
                return lst;
            }
            catch (Exception ex)
            {
                lst.Add(new User() { Name = "Select" });
            }
            return lst;
        }



        public async Task<IEnumerable<User>> GetUserDetailsForAddExenceEdit( int userId, int expMasterID)
        {
            List<User> lst = new List<User>();
            try
            {

                var ExpenceUserDetails = from details in _dbContext.ExpDetails
                                         join user in _dbContext.LoginDetails on details.ExpenceUserID equals user.UserId
                                         where details.ExpenceDetailsExpID == expMasterID
                                         select new User
                                         {
                                             Id = user.UserId,
                                             Name = user.UserName,
                                             Amount = details.ExpencePersPrice,
                                             ExpDetailsId = details.ExpenceDetailsID
                                         };
                                        

                

                return await ExpenceUserDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                lst.Add(new User() { Name = "Select" });
            }
            return lst;
        }



        public async Task<AddExpence> GetMainExpenceDetails(int userId, int expMasterID)
        {
            try
            {
                var expenceUserDetails = await (from masterD in _dbContext.ExpMaster
                                                join user in _dbContext.LoginDetails on masterD.ExpenceUserId equals user.UserId
                                                where masterD.ExpenceID == expMasterID
                                                select new AddExpence
                                                {
                                                    Amount = masterD.ExpensePrice,
                                                    ExpenceDate = masterD.ExpenceDate, // Default date handling
                                                    SelectedCategory = masterD.ExpenceCategory, // Default string handling
                                                    Description = masterD.ExpenceDescription ?? string.Empty,
                                                    UserId = user.UserId
                                                }).FirstOrDefaultAsync();

                return expenceUserDetails ?? new AddExpence();
            }
            catch (Exception ex)
            {
                new AddExpence();
            }
            return new AddExpence();
        }



        //public async Task<DownloadLastBill> GetExpenceDetailsBill(DateTime fromDate, DateTime toDate, int Userid)
        //{
        //    DownloadLastBill downloadLastBill = new DownloadLastBill();
        //    //        DateTime adjustedToDate = toDate.Date.AddDays(1).AddTicks(-1);

        //    try
        //    {

        //            var FamilyId = _dbContext.LoginDetails
        //        .FirstOrDefault(x => x.UserId == Userid).UserFamilyId;

        //            var expenseSummary =
        //from x in _dbContext.ExpDetails
        //where x.ExpenceUserID != x.ParentUserId 
        //      && x.ExpenceFamilyId == FamilyId 
        //      && x.ExpenceDetailsEntryDate >= fromDate
        //      && x.ExpenceDetailsEntryDate <= adjustedToDate
        //group x by new
        //{
        //    user1 = x.ExpenceUserID < x.ParentUserId ? x.ExpenceUserID : x.ParentUserId,
        //    user2 = x.ExpenceUserID < x.ParentUserId ? x.ParentUserId : x.ExpenceUserID
        //} into grouped
        //select new
        //{
        //    user_1 = grouped.Key.user1,
        //    user_2 = grouped.Key.user2,
        //    user_1_to_user_2_expense = grouped.Where(e => e.ExpenceUserID < e.ParentUserId).Sum(e => e.ExpencePersPrice),
        //    user_2_to_user_1_expense = grouped.Where(e => e.ExpenceUserID > e.ParentUserId).Sum(e => e.ExpencePersPrice)
        //};


        //            var result =
        //                from es in expenseSummary
        //                join u1 in _dbContext.LoginDetails on es.user_1 equals u1.UserId
        //                join u2 in _dbContext.LoginDetails on es.user_2 equals u2.UserId
        //                where u1.UserFamilyId==FamilyId
        //                select new
        //                {
        //                    user_1_name = u1.UserName,
        //                    user_2_name = u2.UserName,
        //                    net_amount = es.user_1_to_user_2_expense - es.user_2_to_user_1_expense
        //                };


        //            var formattedResult = result
        //                .AsEnumerable()  // Switch to client-side
        //                .Select(r => new
        //                {
        //                    PaymentDetails = r.net_amount < 0
        //                        ? $"{r.user_2_name} needs to pay {Math.Abs(r.net_amount)} to {r.user_1_name}"
        //                        : $"{r.user_1_name} needs to pay {r.net_amount} to {r.user_2_name}"
        //                })
        //                .ToList();
        //            formattedResult = formattedResult
        //                .Where(x => !x.PaymentDetails.Contains("0.0"))
        //                .ToList();


        //            downloadLastBill.lstExprackMap = new List<ExprackMap>();
        //            foreach (var det in formattedResult)
        //            {
        //                downloadLastBill.lstExprackMap.Add(new
        //                    ExprackMap
        //                {
        //                    ExpPaymentDetails = det.PaymentDetails
        //                });
        //            }



        //            var usersUnderFamily = _dbContext.LoginDetails
        //                .Where(x => x.UserFamilyId == FamilyId)
        //                .ToList();

        //            var expenseSums = await (from exp in _dbContext.ExpMaster
        //                                     where usersUnderFamily.Select(u => u.UserId).Contains(exp.ExpenceUserId) && exp.ExpenceFamId==FamilyId && exp.ExpenceDate>=fromDate && exp.ExpenceDate<=adjustedToDate
        //                                     group exp by exp.ExpenceUserId into grouped
        //                                     select new
        //                                     {
        //                                         UserId = grouped.Key,
        //                                         AmountSpent = grouped.Sum(x => x.ExpensePrice)
        //                                     }).ToListAsync();


        //            var expenseSplits = await (from expd in _dbContext.ExpDetails
        //                                       where usersUnderFamily.Select(u => u.UserId).Contains(expd.ExpenceUserID)
        //                                          && expd.ExpenceFamilyId == FamilyId
        //                                          && expd.ExpenceDetailsEntryDate >= fromDate.Date
        //                                          && expd.ExpenceDetailsEntryDate <= adjustedToDate
        //                                       group expd by expd.ExpenceUserID into grouped
        //                                       select new
        //                                       {
        //                                           UserId = grouped.Key,
        //                                           AmountSplitted = grouped.Sum(x => x.ExpencePersPrice)
        //                                       }).ToListAsync();

        //            var result1 = from user in usersUnderFamily
        //                         join exp in expenseSums on user.UserId equals exp.UserId into expGroup
        //                         join split in expenseSplits on user.UserId equals split.UserId into splitGroup
        //                         select new UserExpense
        //                         {
        //                             UserID = user.UserId,
        //                             UserName = user.UserName,
        //                             AmountSpent = expGroup.Sum(x => x.AmountSpent),
        //                             AmountSplitted = splitGroup.Sum(x => x.AmountSplitted)
        //                         };

        //            downloadLastBill.Users = result1.ToList();

        //        DateTime startDate = new DateTime(DateTime.Now.Year, 1, 10);
        //        DateTime endDate = new DateTime(DateTime.Now.Year, 2, 10);

        //        var userTransactions = (from ed in _dbContext.ExpDetails
        //                                join em in _dbContext.ExpMaster on ed.ExpenceDetailsExpID equals em.ExpenceID
        //                                where ed.ExpenceFamilyId == 14 &&
        //                                      em.ExpenceDate >= startDate &&
        //                                      em.ExpenceDate <= endDate
        //                                group ed by new { ed.ParentUserId, ed.ExpenceUserID } into g
        //                                select new
        //                                {
        //                                    OwesUser = g.Key.ParentUserId,
        //                                    PaidByUser = g.Key.ExpenceUserID,
        //                                    Amount = g.Sum(x => x.ExpencePersPrice)
        //                                }).ToList();


        //        var userBalances = (from ut1 in userTransactions
        //                            let paidAmount = userTransactions.Where(ut2 => ut2.OwesUser == ut1.PaidByUser &&
        //                                                                           ut2.PaidByUser == ut1.OwesUser)
        //                                                              .Sum(x => x.Amount)
        //                            select new
        //                            {
        //                                OwesUser = ut1.OwesUser,
        //                                PaidByUser = ut1.PaidByUser,
        //                                Balance = ut1.Amount - (paidAmount)
        //                            }).Where(ub => ub.Balance > 0).ToList();

        //        var result = (from ub in userBalances
        //                      join u1 in _dbContext.LoginDetails on ub.OwesUser equals u1.UserId
        //                      join u2 in _dbContext.LoginDetails on ub.PaidByUser equals u2.UserId
        //                      select new
        //                      {
        //                          TransactionDescription = $"{u1.UserName} needs to pay {Math.Abs(ub.Balance)} to {u2.UserName}"
        //                      }).OrderBy(x => x.TransactionDescription).ToList();




        //        var individualExpensesFromDetails = (from ed in _dbContext.ExpDetails
        //                                             join u in _dbContext.LoginDetails on ed.ExpenceUserID equals u.UserId
        //                                             join em in _dbContext.ExpMaster on ed.ExpenceDetailsExpID equals em.ExpenceID
        //                                             where ed.ExpenceFamilyId == 14 &&
        //                                                   em.ExpenceDate >= startDate &&
        //                                                   em.ExpenceDate <= endDate
        //                                             group ed by new { u.UserId, u.UserName } into g
        //                                             orderby g.Key.UserId
        //                                             select new
        //                                             {
        //                                                 UserId = g.Key.UserId,
        //                                                 UserName = g.Key.UserName,
        //                                                 IndividualTotalExpense = g.Sum(x => x.ExpencePersPrice)
        //                                             }).ToList();

        //        var individualExpensesFromMaster = (from em in _dbContext.ExpMaster
        //                                            join u in _dbContext.LoginDetails on em.ExpenceUserId equals u.UserId
        //                                            where em.ExpenceFamId == 14 &&
        //                                                  em.ExpenceDate >= startDate &&
        //                                                  em.ExpenceDate <= endDate
        //                                            group em by new { u.UserId, u.UserName } into g
        //                                            orderby g.Key.UserId
        //                                            select new
        //                                            {
        //                                                UserId = g.Key.UserId,
        //                                                UserName = g.Key.UserName,
        //                                                IndividualTotalExpense = g.Sum(x => x.ExpensePrice)
        //                                            }).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        return downloadLastBill;
        //    }

        //    return downloadLastBill;
        //}


        public async Task<DownloadLastBill> GetExpenceDetailsBill(DateTime startDate, DateTime endDate, int Userid)
        {
            DownloadLastBill downloadLastBill = new DownloadLastBill();

            try
            {
                var individualExpensesFromDetails = (from ed in _dbContext.ExpDetails
                                                     join u in _dbContext.LoginDetails on ed.ExpenceUserID equals u.UserId
                                                     join em in _dbContext.ExpMaster on ed.ExpenceDetailsExpID equals em.ExpenceID
                                                     where ed.ExpenceFamilyId == 14 &&
                                                           em.ExpenceDate >= startDate &&
                                                           em.ExpenceDate <= endDate
                                                     group ed by new { u.UserId, u.UserName } into g
                                                     orderby g.Key.UserId
                                                     select new
                                                     {
                                                         UserId = g.Key.UserId,
                                                         UserName = g.Key.UserName,
                                                         AmountSpent = g.Sum(x => x.ExpencePersPrice)
                                                     }).ToList();

                // Get expenses each user paid on behalf of others
                var individualExpensesFromMaster = (from em in _dbContext.ExpMaster
                                                    join u in _dbContext.LoginDetails on em.ExpenceUserId equals u.UserId
                                                    where em.ExpenceFamId == 14 &&
                                                          em.ExpenceDate >= startDate &&
                                                          em.ExpenceDate <= endDate
                                                    group em by new { u.UserId, u.UserName } into g
                                                    orderby g.Key.UserId
                                                    select new
                                                    {
                                                        UserId = g.Key.UserId,
                                                        UserName = g.Key.UserName,
                                                        AmountPaid = g.Sum(x => x.ExpensePrice)
                                                    }).ToList();

                // Merging the expenses
                var mergedExpenses = (from detail in individualExpensesFromDetails
                                      join master in individualExpensesFromMaster
                                      on detail.UserId equals master.UserId into joined
                                      from master in joined.DefaultIfEmpty()
                                      select new
                                      {
                                          UserId = detail.UserId,
                                          UserName = detail.UserName,
                                          AmountSpent = detail.AmountSpent,
                                          AmountPaid = master?.AmountPaid ?? 0
                                      }).ToList();

                var userTransactions = (from ed in _dbContext.ExpDetails
                                        join em in _dbContext.ExpMaster on ed.ExpenceDetailsExpID equals em.ExpenceID
                                        where ed.ExpenceFamilyId == 14 &&
                                              em.ExpenceDate >= startDate &&
                                              em.ExpenceDate <= endDate
                                        group ed by new { ed.ParentUserId, ed.ExpenceUserID } into g
                                        select new
                                        {
                                            OwesUser = g.Key.ParentUserId,
                                            PaidByUser = g.Key.ExpenceUserID,
                                            Amount = g.Sum(x => x.ExpencePersPrice)
                                        }).ToList();


                var userBalances = (from ut1 in userTransactions
                                    let paidAmount = userTransactions.Where(ut2 => ut2.OwesUser == ut1.PaidByUser &&
                                                                                   ut2.PaidByUser == ut1.OwesUser)
                                                                      .Sum(x => x.Amount)
                                    select new
                                    {
                                        OwesUser = ut1.OwesUser,
                                        PaidByUser = ut1.PaidByUser,
                                        Balance = ut1.Amount - (paidAmount)
                                    }).Where(ub => ub.Balance > 0).ToList();

                var resultListMap = (from ub in userBalances
                              join u1 in _dbContext.LoginDetails on ub.OwesUser equals u1.UserId
                              join u2 in _dbContext.LoginDetails on ub.PaidByUser equals u2.UserId
                              select new ExprackMap
                              {
                                  ExpPaymentDetails = $"{u2.UserName} needs to pay {Math.Abs(ub.Balance)} to {u1.UserName}"
                              }).OrderBy(x => x.ExpPaymentDetails).ToList();


                // Populate DownloadLastBill with users
                downloadLastBill.Users = mergedExpenses.Select(exp => new UserExpense
                {
                    SerialNumber = exp.UserId,
                    UserName = exp.UserName,
                    AmountSpent = exp.AmountPaid ,
                    UserID = exp.UserId,
                    AmountSplitted = exp.AmountSpent, 
                    ExpenceDate = startDate,
                    
                }).ToList();

                downloadLastBill.FamilyName = "Family Expense Report";
                downloadLastBill.BillMonth = startDate.ToString("MMMM yyyy");
                downloadLastBill.lstExprackMap=resultListMap;
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                return downloadLastBill;
            }

            return downloadLastBill;
        }



        public async Task<FamilyTableDBTypes> GetFamilyInformation(int userId)
        {
            var loginDetail = await _dbContext.LoginDetails.FirstOrDefaultAsync(x => x.UserId == userId);
            if (loginDetail == null)
            {
                return null; 
            }

            int userFamilyId = loginDetail.UserFamilyId;
            var familyDetail = await _dbContext.FamilyTbl.FirstOrDefaultAsync(x => x.FamilyID == userFamilyId);
            return familyDetail; 
        }


        public async Task<IEnumerable<LoginDBTypes>> GetUserDetailsUnderFamily(int familyId)
        {
            try
            {
                var loginDetails = await _dbContext.LoginDetails
                                                   .Where(x => x.UserFamilyId == familyId)
                                                   .ToListAsync();
                return loginDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<IEnumerable< Models.UserMaster.Transaction>> GetExpenceReportDetails(int UserId,DateTime fromDate,DateTime toDate)
        {
            try
            {
                var loginDetails = await _dbContext.LoginDetails.Where(x => x.UserId == UserId   ).ToListAsync();
                int FamilyId = loginDetails.FirstOrDefault().UserFamilyId;

                var result = await (from exp in _dbContext.ExpMaster
                                    join login in _dbContext.LoginDetails on exp.ExpenceUserId equals login.UserId
                                    join family in _dbContext.FamilyTbl on login.UserFamilyId equals family.FamilyID
                                    join cate in _dbContext.CategoryTable on exp.ExpenceCategory equals cate.CategoryId
                                    where exp.ExpenceDate >= fromDate && exp.ExpenceDate <= toDate && family.FamilyID== FamilyId
                                    select new Models.UserMaster.Transaction
                                    {
                                        UserName = login.UserName,
                                        Amount = exp.ExpensePrice,
                                        Description = exp.ExpenceDescription,
                                        Date= exp.ExpenceDate,
                                        FamilyName= family.FamilyName,
                                        SplitDetails = _dbContext.ExpDetails
                                            .Where(d => d.ExpenceDetailsExpID == exp.ExpenceID) 
                                            .Select(d => new Models.UserMaster.UserSplittedDetails
                                            {
                                                UserName =  _dbContext.LoginDetails.Where(x=>x.UserId==d.ExpenceUserID).FirstOrDefault().UserName, 
                                                Amount = d.ExpencePersPrice
                                            }).ToList()
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<SelectListItem> AddExpenceAsync(AddExpence model)
        {
            try
            {
                if(model.ExpenceMasterId>0)
                {
                    var familyIdUpdate = await _dbContext.LoginDetails.Where(x => x.UserId == model.UserId).Select(x => x.UserFamilyId).FirstOrDefaultAsync();
                    if (model.ExpenceDate < SqlDateTime.MinValue.Value || model.ExpenceDate > SqlDateTime.MaxValue.Value)
                    {
                        model.ExpenceDate = DateTime.Now; // or another fallback value
                    }

                    var masterRecordUpdate = new ExpenseMasterTableDBTypes
                    {
                        ExpenceName = model.Description,
                        ExpensePrice = model.Amount,
                        ExpenceCategory = model.SelectedCategory,
                        ExpenceDate = model.ExpenceDate,
                        ExpenceUserId = model.UserId,
                        ExpenceFamId = familyIdUpdate,
                        ExpenceDescription = model.Description,
                        ExpenceEntryDate = DateTime.Now,
                        ExpenceID=model.ExpenceMasterId
                    };

                     _dbContext.ExpMaster.Update(masterRecordUpdate);
                    await _dbContext.SaveChangesAsync();


                    foreach (var user in model.Users)
                    {
                        var expenseDetail = new ExpenseDetailsTableDBTypes
                        {
                            ParentUserId=model.UserId,
                            ExpenceDetailsID = user.ExpDetailsId,
                            ExpenceDetailsExpID = masterRecordUpdate.ExpenceID,
                            ExpenceUserID = user.Id,
                            ExpencePersPrice = user.Amount,
                            ExpenceFamilyId = familyIdUpdate,
                            ExpenceDetailsEntryDate = DateTime.Now
                        };

                         _dbContext.ExpDetails.Update(expenseDetail);
                    }
                    await _dbContext.SaveChangesAsync();
                    return new SelectListItem
                    {
                        Text = "Expense Updated Successfully",
                        Value = masterRecordUpdate.ExpenceID.ToString()
                    };
                }

                var familyId = await _dbContext.LoginDetails.Where(x => x.UserId == model.UserId).Select(x => x.UserFamilyId).FirstOrDefaultAsync();
                if (model.ExpenceDate < SqlDateTime.MinValue.Value || model.ExpenceDate > SqlDateTime.MaxValue.Value)
                {
                    model.ExpenceDate = DateTime.Now; // or another fallback value
                }

                var masterRecord = new ExpenseMasterTableDBTypes
                {
                    ExpenceName = model.Description,   
                    ExpensePrice = model.Amount,
                    ExpenceCategory = model.SelectedCategory,
                    ExpenceDate = model.ExpenceDate,
                    ExpenceUserId = model.UserId,
                    ExpenceFamId = familyId,
                    ExpenceDescription = model.Description,
                    ExpenceEntryDate = DateTime.Now 
                };

                await _dbContext.ExpMaster.AddAsync(masterRecord);
                await _dbContext.SaveChangesAsync();


                foreach (var user in model.Users)
                {
                    var expenseDetail = new ExpenseDetailsTableDBTypes
                    {
                        ParentUserId = model.UserId,
                        ExpenceDetailsExpID = masterRecord.ExpenceID, 
                        ExpenceUserID = user.Id,  
                        ExpencePersPrice = user.Amount, 
                        ExpenceFamilyId = familyId,
                        ExpenceDetailsEntryDate = DateTime.Now  
                    };

                    await _dbContext.ExpDetails.AddAsync(expenseDetail);
                }
                await _dbContext.SaveChangesAsync();
                return new SelectListItem
                {
                    Text = "Expense Added Successfully", 
                    Value = masterRecord.ExpenceID.ToString()
                };

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the expense", ex);
            }
        }


        public async Task<SelectListItem> DeleteExpense(int userId, int expMasterId)
        {
            try
            {
                var expDetails = await _dbContext.ExpDetails
                    .Where(x => x.ExpenceDetailsExpID == expMasterId).ToListAsync();

                if (expDetails.Any())
                {
                    _dbContext.ExpDetails.RemoveRange(expDetails);
                    await _dbContext.SaveChangesAsync();

                }

                var expMaster = await _dbContext.ExpMaster
                    .FirstOrDefaultAsync(x => x.ExpenceID == expMasterId);

                if (expMaster != null)
                {
                    _dbContext.ExpMaster.Remove(expMaster);
                    await _dbContext.SaveChangesAsync();

                }


                return new SelectListItem
                {
                    Text = "Expense deleted successfully",
                    Value = expMasterId.ToString()
                };
            }
            catch (Exception ex)
            {
                // Log and rethrow exception with meaningful message
                throw new InvalidOperationException("An error");
        }
        }



    }
        }
