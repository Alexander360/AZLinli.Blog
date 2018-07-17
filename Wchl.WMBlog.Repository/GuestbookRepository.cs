using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.IRepository;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Repository.Base;

namespace Wchl.WMBlog.Repository
{
    public class GuestbookRepository : BaseRepository<Guestbook>, IGuestbookRepository
    {
    }
}
