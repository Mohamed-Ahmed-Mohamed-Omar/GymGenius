using AutoMapper;
using GymGenius.Data.Entities;
using GymGenius.Models.Advertisements;
using GymGenius.Models.Notifications;
using GymGenius.Models.Offers;
using GymGenius.Models.Plans;
using GymGenius.Models.Shape;
using GymGenius.Models.Shapes;
using GymGenius.Models.Subscriptions;

namespace GymGenius.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {

            #region Offer Settings 

            CreateMap <Offer, CreateOffer>().ReverseMap();

            CreateMap <Offer, GetAllOffers>().ReverseMap();

            CreateMap <Offer, UpdateOfffer>().ReverseMap();

            CreateMap <Offer, GetOfferDerails>().ReverseMap();

            #endregion

            #region Notification Settings

            CreateMap <Notification, CreateNotification>().ReverseMap();

            CreateMap <Notification, GetAllNotifications>().ReverseMap();

            #endregion

            #region Advertisement Settings

            CreateMap <Advertisement, CreateAdvertisement>().ReverseMap();

            CreateMap <Advertisement, GetAllAdvertisements>().ReverseMap();

            CreateMap <Advertisement, UpdateAdvertisement>().ReverseMap();

            CreateMap <Advertisement, GetAdvertisementDetails>().ReverseMap();

            #endregion

            #region Shape Settings

            CreateMap <Shape_Training, CreateShape>().ReverseMap();

            CreateMap <Shape_Training, GetAllShapes>().ReverseMap();

            CreateMap <Shape_Training, GetAllNameTraining>().ReverseMap();

            #endregion

            #region Plan Settings 

            CreateMap <Plan, CreatePlan>().ReverseMap();

            CreateMap <Plan, GetPlanDetails>().ReverseMap();

            CreateMap <Plan, UpdatePlan>().ReverseMap();

            #endregion

            #region Subscription Settings

            CreateMap <Subscription, CreateSubscription>().ReverseMap();

            CreateMap <Subscription, GetAllSubscriptions>().ReverseMap();

            CreateMap <Subscription, GetSubscriptionDetails>().ReverseMap();

            CreateMap <SubscriptionDay, SubscriptionDayGetDetauls>().ReverseMap();

            CreateMap <Subscription, SubscriptionDayGetDetauls>().ReverseMap();

            CreateMap <SubscriptionDay, UpdateSubscription>().ReverseMap();

            CreateMap <SubscriptionDay, UpdateSubscriptionToUser>().ReverseMap();

            #endregion
        }
    }
}
