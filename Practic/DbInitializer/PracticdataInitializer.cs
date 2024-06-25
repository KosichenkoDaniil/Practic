using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;




namespace Practic.DbInitializer
{
    public class PracticdataInitializer
    {
        public static void Initialize(PracticdataContext db)
        {
            db.Database.EnsureCreated();

        }

    }
}
