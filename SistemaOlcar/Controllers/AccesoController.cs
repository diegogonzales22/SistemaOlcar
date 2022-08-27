using SistemaOlcar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace SistemaOlcar.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario1, string contraseña)
        {
            using (OLCAREntities db = new OLCAREntities())
            {
                var oUser = (from d in db.Usuario
                             where d.usuario1 == usuario1.Trim() && d.contraseña == contraseña.Trim()
                             select d).FirstOrDefault();
                if (oUser != null)
                {
                    if (oUser.estado == false)
                    {
                        TempData["error"] = "El usuario está desactivado";
                        return View();
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(oUser.usuario1, false);
                        Session["Usuario"] = oUser;
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else
                {
                    TempData["error"] = "Usuario y/o password incorrecta";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;
            return RedirectToAction("Login");
        }
    }
}