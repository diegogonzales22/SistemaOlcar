using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;
using SistemaOlcar.Helpers;
using System.Net;
using System.Data.Entity;

namespace SistemaOlcar.Controllers
{
    public class UsuarioController : Controller
    {
        OLCAREntities db = new OLCAREntities();
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar() //Listar Usuario
        {
            List<TableUsuario> oLstUusuario = new List<TableUsuario>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstUusuario = (from p in d.Usuario 
                                select new TableUsuario
                                {
                                    idUsuario = p.idUsuario,
                                    usuario = p.usuario1,
                                    rol = p.Rol.nombre,
                                    estado = p.estado
                                }
                                ).ToList();
            }
            return Json(new { data = oLstUusuario }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Crear()
        {
            ViewBag.idRol = new SelectList(db.Rol, "idRol", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Crear([Bind(Include = "idUsuario,usuario1,contraseña,idRol,estado")] Usuario usuario)
        {
            OLCAREntities bd = new OLCAREntities();
            bool existe = bd.Usuario.Any(x => x.usuario1 == usuario.usuario1);

            try
            {
                if (ModelState.IsValid)
                {
                    //db.Configuration.ProxyCreationEnabled = false;
                    Usuario oUsuario = new Usuario();
                    oUsuario.contraseña = Encrypt.EncriptarMD5(usuario.contraseña);
                    oUsuario.usuario1 = usuario.usuario1;
                    oUsuario.estado = usuario.estado;
                    oUsuario.idRol = usuario.idRol;

                    if (existe == true)
                    {
                        ViewBag.error = "El usuario que pretende ingresar ya existe";
                        ViewBag.idRol = new SelectList(db.Rol, "idRol", "nombre");
                        return View(usuario);
                    }
                    else
                    {
                        using (OLCAREntities db = new OLCAREntities())
                        {
                            db.Usuario.Add(oUsuario);
                            db.SaveChanges();
                            TempData["exito"] = "El usuario fue registrado con éxito";
                        }
                        ViewBag.idRol = new SelectList(db.Rol, "idRol", "nombre");
                        return RedirectToAction("Crear", "Usuario");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(usuario);
        }

        public JsonResult Details(int idUsuario) //Detalles de usuario
        {
            TableUsuario oUsuario = new TableUsuario();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oUsuario = (from p in d.Usuario
                              where p.idUsuario == idUsuario
                              select new TableUsuario { 
                                idUsuario = p.idUsuario,
                                usuario = p.usuario1,
                                contraseña = p.contraseña,
                                rol = p.Rol.nombre,
                                estado = p.estado
                              }).FirstOrDefault();
            }
            return Json(oUsuario, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRol = new SelectList(db.Rol, "idRol", "nombre", usuario.idRol);
            return View(usuario);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        usuario.contraseña = Encrypt.EncriptarMD5(usuario.contraseña);
                        db.Entry(usuario).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["exito"] = "El proveedor fue modificado con éxito";
                    }
                    return RedirectToAction("Editar", new { id = usuario.idUsuario });

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(usuario);
        }

    }
}