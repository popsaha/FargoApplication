using Fargo_DataAccessLayers;
using FargoWebApplication.Filter;
using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Manager;
using System.Threading;
using System.Web.Http;
using System.Collections.Generic;
using System;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class MaterialAPIController : ApiController
    {
        [HttpGet]
        [Route("api/MaterialAPI/LstMaterials")]
        public IHttpActionResult LstMaterials()
        {
            List<MaterialModel> LstMaterials = new List<MaterialModel>();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstMaterials = MaterialManager.LstMaterials();

                    if (LstMaterials.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    return Ok(LstMaterials);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return InternalServerError();
            }
        }

        
    }
}
