using System.Collections.Generic;

namespace FAIS.Models
{
    public class BORepository
    {
        // private FAISEntities db = new FAISEntities();
        private SGBD s = new SGBD();
        // private BORepositoryGenerator Gen { get; set; }
        public META_BO MetaBO { get; set; }

        public bool ExecInsert(string statement, Dictionary<string, object> items)
        {
            return s.Insert(statement, items);

        }

        public bool ExecUpdate(string statement, Dictionary<string, object> items)
        {
            return s.Update(statement, items);

        }

 


        //public BORepository(META_BO metaBO)
        //{
        //    this.MetaBO = metaBO;
        //    Gen = new BORepositoryGenerator(MetaBO);

        //}

        //public object Select()
        //{

        //    var dt = s.Cmd(Gen.GenSelect());
        //    // TODO : SGBD call Cmd
        //    return dt;

        //}

        //public bool Insert(string insetStatements)
        //{
        //    var dt = Gen.GenInsert(insetStatements);
        //    return true;
        //}
    }


    public class BORepositoryGenerator
    {
        private SGBD s = new SGBD();
        private META_BO metaBO;

        public BORepositoryGenerator(META_BO metaBO)
        {
            this.metaBO = metaBO;
        }

        public string GenSelect()
        {
            // TODO : Filter
            string select = "";
            select = "select * from " + this.metaBO.BO_NAME + " ";

            return select;
        }

        // TODO : GenInsert

        public string GenInsert(string insetStatements)
        {
            return string.Format("insert into {0} {1} array()", this.metaBO.BO_NAME, insetStatements);
        }


        // TODO : GenUpdate
        // TODO : GenDelete


    }
}
