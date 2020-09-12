using System;
using System.Linq;
using System.Collections.Generic;
using CoWorking.App.Models;
using CoWorking.App.Models.Enumerations;

namespace CoWorking.App.Data{

    public class DeskData{
        private JsonManager<Desk> jsonManager;

        public DeskData()
        {
            jsonManager = new JsonManager<Desk>();
        }

        public bool CreateDesk(Desk desk){
            var deskCollection = jsonManager.GetCollection();

            deskCollection.Add(desk);

            jsonManager.SaveCollection(deskCollection);

            return true;
        }

        

        public bool EditDesk(Desk desk){
            var deskCollection = jsonManager.GetCollection();

            var indexDesk = deskCollection.FindIndex(p=>p.DeskId == desk.DeskId);

            deskCollection[indexDesk] = desk;

            jsonManager.SaveCollection(deskCollection);

            return true;
        }

        public bool DeleteDesk(Guid deskId){
            var deskCollection = jsonManager.GetCollection();

            var indexDesk = deskCollection.FindIndex(p=>p.DeskId == deskId);

            deskCollection.RemoveAt(indexDesk);

            jsonManager.SaveCollection(deskCollection);

            return true;
        }

        public Desk FindDesk(string numberDesk)
        {
            var deskCollection = jsonManager.GetCollection();
            return deskCollection.FirstOrDefault(p=>p.Number == numberDesk);
        }

        public IEnumerable<Desk> GetAvailableDesk()
        {
            return jsonManager.GetCollection().Where(p=> p.DeskStatus == Models.Enumerations.DeskStatus.Active);
        }

        public IEnumerable<Desk> GetAllDesks()
        {
            return jsonManager.GetCollection();
        }

    }


}