using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Category
{
    public class CategoryTableDBTypes
    {
        [Key]
        [Column("exp_id")]
        public int CategoryId { get; set; }

        [Column("exp_name")]
        public string CatagoryName { get; set; }

        [Column("exp_desc")]
        public string CatergoryDescription { get; set; }
    }
}
