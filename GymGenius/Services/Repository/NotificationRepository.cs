using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Notifications;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateNotification entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var data = _mapper.Map<Notification>(entity);

                await _context.notifications.AddAsync(data);

                RG.Done = true;

                RG.Message = "Done";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                RG.Done = false;

                RG.Message = $"An error occurred: {ex.Message}";
            }

            return RG;
        }

        public async Task<ResponseGeneral> DeleteAsync(int id)
        {
            var delData = await _context.notifications.FindAsync(id);

            if (delData != null)
            {
                _context.notifications.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<IEnumerable<GetAllNotifications>> ListAllAsync()
        {
            var notifications = await _context.notifications.ToListAsync();

            var mappednotifications = _mapper.Map<IEnumerable<GetAllNotifications>>(notifications);

            return mappednotifications;
        }
    }
}
