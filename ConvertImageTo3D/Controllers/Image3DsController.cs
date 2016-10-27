using ConvertImageTo3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Web.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace ConvertImageTo3D.Controllers
{
    public class Image3DsController : ApiController
    {
        Image3DEntities db = new Image3DEntities();

        // GET api/Image3Ds/[id]
        public ImageTB Get(int id)
        {
            ImageTB imgTb = db.ImageTBs.Single(p => p.id == id);
            return imgTb;
        }

        // PUT api/Image3Ds/[id]
        public bool Put(int id, [FromBody]ImageTB imgClient)
        {
            bool result = false;
            try
            {
                ImageTB img = db.ImageTBs.Single(p => p.id == id);
                if (img != null)
                {
                    /*using (Image image = Base64ToImage(imgClient.imgStr))
                    {
                        string strFileName = "~/Image/"+ "pdk.jpeg";
                        image.Save(HttpContext.Current.Server.MapPath(strFileName), ImageFormat.Jpeg);
                        img.imgStr = ImageToBase64(image, ImageFormat.Jpeg);
                    }*/
                    img.img = imgClient.img;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        // POST api/values/
        public bool Post([FromBody]ImageTB imgClient)
        {
            try
            {
                ImageTB img = new ImageTB();
                img.img = imgClient.img;
                db.ImageTBs.Add(img);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private string ImageToBase64(Image image, ImageFormat format)
        {
            string base64String;
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                ms.Position = 0;
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);
            }
            return base64String;
        }

        private Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            Bitmap tempBmp;
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                using (Image image = Image.FromStream(ms, true))
                {
                    //Create another object image for dispose old image handler
                    tempBmp = new Bitmap(image.Width, image.Height);
                    Graphics g = Graphics.FromImage(tempBmp);
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                }
            }
            return tempBmp;
        }

    }
}
