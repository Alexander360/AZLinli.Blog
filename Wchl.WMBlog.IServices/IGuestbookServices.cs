﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.IServices.Base;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;

namespace Wchl.WMBlog.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        List<GuestbookViewModels> getGuestbook(int id);
    }
}