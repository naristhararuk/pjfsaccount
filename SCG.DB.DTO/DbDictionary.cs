using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
   public class DbDictionary
    {
       private int id;
       private string verb1;
       private string verb3;
       private string verb3Thai;
       public DbDictionary()
       {

       }
       public DbDictionary(int id)
       {
           this.id = id;
       }
       public DbDictionary(string verb1)
       {
           this.verb1 = verb1;
       }
       public DbDictionary(int id,string verb1,string verb3)
       {
           this.verb1 = verb1;
           this.verb3 = verb3;
       }
       public virtual int ID
       {
           get
           {
               return this.id;
           }
           set
           {
               this.id = value;
           }
       }

       public virtual string Verb1
       {
           get
           {
               return this.verb1;
           }
           set
           {
               this.verb1 = value;
           }
       }

       public virtual string Verb3
       {
           get
           {
               return this.verb3;
           }
           set
           {
               this.verb3 = value;
           }
       }
       public virtual string Verb3Thai
       {
           get
           {
               return this.verb3Thai;
           }
           set
           {
               this.verb3Thai = value;
           }
       }
    }
}
