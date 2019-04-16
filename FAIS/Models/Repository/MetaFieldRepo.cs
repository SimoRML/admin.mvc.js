using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAIS.Models.Repository
{
    public class MetaFieldRepo
    {
        private FAISEntities db = new FAISEntities();
        public META_FIELD Create(META_FIELD mETA_FIELD, string userName)
        {
            mETA_FIELD.DB_NAME = Helper.cleanDBName(mETA_FIELD.DB_NAME);
            mETA_FIELD.CREATED_BY = userName;
            mETA_FIELD.UPDATED_BY = userName;
            mETA_FIELD.STATUS = "NEW";

            return mETA_FIELD;
        }
        public async System.Threading.Tasks.Task<META_FIELD> CreateAndSaveAsync(META_FIELD mETA_FIELD, string userName)
        {
            mETA_FIELD = Create(mETA_FIELD, userName);
            db.META_FIELD.Add(mETA_FIELD);
            await db.SaveChangesAsync();
            return mETA_FIELD;
        }

        public dynamic GetDefaultValue(string format, string boName, int persist)
        {
            string cle = "", formule = "", step = "", type = "";
            bool inFomule = false;
            int count = 0;

            foreach (char car in format)
            {
                if (car == '[')
                {
                    formule += car;
                    inFomule = true;
                    continue;
                }
                if (car == ']')
                {
                    formule += car;
                    inFomule = false;
                    continue;
                }

                if (inFomule)
                {
                    formule += car;
                    count++;
                    if (count == 1)
                    {
                        switch (car)
                        {
                            case '+':
                                type = "plus";
                                break;
                            case 'd':
                                type = "date";
                                break;
                        }
                    }
                    else
                    {
                        step += car;
                    }
                }
                else
                {
                    cle += car;
                }
            }

            switch (type)
            {
                case "plus":
                    var rst = db.PlusSequenceNextID(cle, boName, int.Parse(step)).ToList()[0];
                    return new { type, value = format.Replace(formule, rst.ToString()) };
                case "date":
                    return new { type, value = step == "" ? DateTime.Now : DateTime.Now.AddDays(int.Parse(step)) };
                default:
                    return new { type = "error", msg = "Formule non prise en charge !" };
            }
        }
    }
}