using System.ComponentModel.DataAnnotations;

namespace MVC_Tutorial.Models
{
    public class Expense
    {
        #region Members
        #endregion

        #region Constructor
        #endregion

        #region Methods
        public int Id { get; set; }
        public decimal Value { get; set; }

        [Required]
        public string? Description { get; set; }
        #endregion
    }
}
