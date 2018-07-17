﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Common.ToolsHelper;
using Wchl.WMBlog.IRepository;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;
using Wchl.WMBlog.Services.Base;

namespace Wchl.WMBlog.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        IAdvertisementRepository dal;

        public AdvertisementServices(IAdvertisementRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
    }
}