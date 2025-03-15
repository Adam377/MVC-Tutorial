using Microsoft.EntityFrameworkCore;

namespace MVC_Tutorial.Models
{
    public class MVCTutorialDbContext : DbContext
    {
        #region Members
        #endregion

        #region Constructor
        public MVCTutorialDbContext(DbContextOptions<MVCTutorialDbContext> options) : base(options)
        {
                
        }
        #endregion

        #region Methods
        public DbSet<Expense> Expenses { get; set; }
        #endregion
    }
}
