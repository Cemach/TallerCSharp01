using System;
using CoWorking.App.Models;
using CoWorking.App.Data;

namespace CoWorkingApp.Data
{
    public class ReservationData
    {

        private JsonManager<Reservation> jsonManager;

        public ReservationData(){
            jsonManager = new JsonManager<Reservation>();
        }

        public void CreateReservation(Reservation newReservation)
        {
            var reservationCollection = jsonManager.GetCollection();
            reservationCollection.Add(newReservation);

            jsonManager.SaveCollection(reservationCollection);
        }
    }
}