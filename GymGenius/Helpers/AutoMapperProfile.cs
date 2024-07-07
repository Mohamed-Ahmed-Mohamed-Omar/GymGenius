using AutoMapper;
using GymGenius.Data.Entities;
using GymGenius.Models.Advertisements;
using GymGenius.Models.Notifications;
using GymGenius.Models.Offers;
using GymGenius.Models.Plans;
using GymGenius.Models.Products;
using GymGenius.Models.Rates;
using GymGenius.Models.Shape;
using GymGenius.Models.Subscriptions;
using GymGenius.Models.TrackProgresses;

namespace GymGenius.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            #region Offer Settings 

            CreateMap<Offer, CreateOffer>().ReverseMap();

            CreateMap<Offer, GetAllOffers>().ReverseMap();

            CreateMap<Offer, UpdateOfffer>().ReverseMap();

            #endregion

            #region Notification Settings

            CreateMap<Notification, CreateNotification>().ReverseMap();

            CreateMap<Notification, GetAllNotifications>().ReverseMap();

            #endregion

            #region Advertisement Settings

            CreateMap<Advertisement, CreateAdvertisement>().ReverseMap();

            CreateMap<Advertisement, GetAllAdvertisements>().ReverseMap();

            CreateMap<Advertisement, UpdateAdvertisement>().ReverseMap();

            #endregion

            #region Shape Settings

            CreateMap<Shape_Training, CreateShape>().ReverseMap();

            CreateMap<Shape_Training, GetAllShapes>().ReverseMap();

            #endregion

            #region Plan Settings 

            CreateMap<Plan, CreatePlan>().ReverseMap();

            CreateMap<Plan, GetAllPlansByPlaceandGoal>().ReverseMap();

            CreateMap<Plan, GetAllPlans>().ReverseMap();

            CreateMap<Plan, UpdatePlan>().ReverseMap();

            #endregion

            #region Subscription Settings

            CreateMap<Subscription, CreateSubscription>().ReverseMap();

            CreateMap<Subscription, GetSubscriptionDetails>().ReverseMap();

            CreateMap<Subscription, UpdateSubscription>().ReverseMap();

            CreateMap<Subscription, UpdateSubscriptionToUser>().ReverseMap();

            #endregion

            #region Track Progress Settings

            CreateMap<Track_Progress, CreateTrackProgress>().ReverseMap();

            CreateMap<Track_Progress, GetAllTrackProgress>().ReverseMap();

            #endregion

            #region Products Settings 

            CreateMap<Product, CreateProduct>().ReverseMap();

            CreateMap<Product, GetAllProducts>().ReverseMap();

            #endregion

            #region Rates Settings 

            CreateMap <Rate, CreateRate>().ReverseMap();

            CreateMap <Rate, GetAllRates>().ReverseMap();

            #endregion
        }
    }
}
