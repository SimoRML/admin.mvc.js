using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FAIS.Models;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/Bon")]
    public class ApiBonController : ApiController
    {

        [HttpGet]
        [Route("Describe/{id}")]
        public async Task<IHttpActionResult> Describe([FromUri]string id)
        {
            var db = new FAISEntities();

            string req = "SELECT column_name AS[Field], DATA_TYPE  AS[Type], IS_NULLABLE AS[Null], " +
                "case when exists(select* FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE  " +
                "WHERE  column_name = c.column_name and table_name = c.table_name " +
                "and CONSTRAINT_NAME like 'PK_%') then 'PRI' else '' end as [Key], " +
                    "COLUMN_DEFAULT as [Default] ,case when COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, " + "'IsIdentity') = 1 then 'auto_increment' else '' end as [Extra] " +
                " FROM INFORMATION_SCHEMA.Columns c                WHERE table_name = '" + id + "'";

            // var Data = await db.Database.SqlQuery<DescribeTable>(req).ToListAsync();
            // return Ok(Data);
            return Ok();

        }
    }
}
