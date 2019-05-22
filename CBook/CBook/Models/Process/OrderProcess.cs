﻿using CBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBook.Models.Process
{
    public class OrderProcess
    {
        //khởi tạo dữ liệu từ tầng data
        CsK23T1cEntities db = null;

        //contructor
        public OrderProcess()
        {
            db = new CsK23T1cEntities();
        }

        /// <summary>
       
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
    
        /// <summary>
        
        /// </summary>
        /// <returns>List</returns>
        public List<DonDatHang> ListOrder()
        {
            return db.DonDatHangs.OrderBy(x => x.MaDDH).ToList();
        }

        /// <summary>
        /// hàm thêm đơn hàng
        /// </summary>
        /// <param name="order">DonDatHang</param>
        /// <returns>int</returns>
        public int Insert(DonDatHang order)
        {
            db.DonDatHangs.Add(order);
            db.SaveChanges();
            return order.MaDDH;
        }
    }
}