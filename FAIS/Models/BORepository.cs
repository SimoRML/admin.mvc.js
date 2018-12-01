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

        public string ExecUpdate(string statement, Dictionary<string, object> items)
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



        public string GenSelect(string Tname)
        {
            // TODO : Filter 
            string select = "";
            select = "select * from VIEW_" + Tname + " ";

            return select;
        }

        // TODO GenSelectOne (Switch to filter Model)

        public string GenSelectOne(string Tname)
        {
            // TODO : Filter 
            string select = "";
            select = "select * from VIEW_" + Tname + " where BO_ID=@BO_ID";

            return select;
        }

        // TODO : GenUpdate
        // TODO : GenDelete


    }
}
