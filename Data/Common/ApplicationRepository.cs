using Marketly.Data.Common;
using Marketly.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Common
{
    public class ApplicationRepository : BaseRepository, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
