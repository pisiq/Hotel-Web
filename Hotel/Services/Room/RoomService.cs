using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IWebHostEnvironment _environment;
        private const string PhotosDirectory = "room-photos";

        public RoomService(IRoomRepository roomRepository, IWebHostEnvironment environment)
        {
            _roomRepository = roomRepository;
            _environment = environment;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRoomsWithDetailsAsync()
        {
            return await _roomRepository.GetAllWithDetailsAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<Room?> GetRoomByIdWithDetailsAsync(int id)
        {
            return await _roomRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<int> AddRoomAsync(Room room, IFormFile? mainPhoto, List<IFormFile>? additionalPhotos)
        {
            try
            {
                // Upload main photo if provided
                if (mainPhoto != null)
                {
                    // Check file size
                    if (mainPhoto.Length > 10 * 1024 * 1024) // 10MB limit
                        throw new Exception("Main photo exceeds the maximum allowed size (10MB).");

                    room.MainPhotoPath = await SavePhotoAsync(mainPhoto);
                }

                // Save the room first to get the ID
                await _roomRepository.AddAsync(room);

                // Upload additional photos if provided
                if (additionalPhotos != null && additionalPhotos.Count > 0)
                {
                    // Check each file size
                    foreach (var photo in additionalPhotos)
                    {
                        if (photo.Length > 10 * 1024 * 1024) // 10MB limit
                            throw new Exception("One of the additional photos exceeds the maximum allowed size (10MB).");
                    }

                    await SaveAdditionalPhotosAsync(room, additionalPhotos);
                }

                return room.Id;
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error adding room: {ex.Message}");
                throw; // Rethrow to be handled by the controller
            }
        }

        public async Task<bool> UpdateRoomAsync(Room room, IFormFile? mainPhoto, List<IFormFile>? additionalPhotos)
        {
            try
            {
                // Get the existing room to compare
                var existingRoom = await _roomRepository.GetByIdWithDetailsAsync(room.Id);
                if (existingRoom == null)
                {
                    return false;
                }

                // Upload new main photo if provided
                if (mainPhoto != null)
                {
                    // Delete old photo if exists
                    DeletePhotoIfExists(existingRoom.MainPhotoPath);

                    // Save new photo
                    room.MainPhotoPath = await SavePhotoAsync(mainPhoto);
                }
                else
                {
                    // Keep the existing main photo
                    room.MainPhotoPath = existingRoom.MainPhotoPath;
                }

                // Update the room entity
                await _roomRepository.UpdateAsync(room);

                // Upload additional photos if provided
                if (additionalPhotos != null && additionalPhotos.Count > 0)
                {
                    await SaveAdditionalPhotosAsync(room, additionalPhotos);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            try
            {
                // Get room with details first
                var room = await _roomRepository.GetByIdWithDetailsAsync(id);
                if (room == null)
                {
                    return false;
                }

                // Delete main photo if exists
                DeletePhotoIfExists(room.MainPhotoPath);

                // Delete all associated photos
                foreach (var photo in room.Photos)
                {
                    DeletePhotoIfExists(photo.FilePath);
                }

                // Delete the room (which will cascade delete photos due to FK constraint)
                await _roomRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateRoomPhotoAsync(RoomPhoto photo, IFormFile? file)
        {
            try
            {
                if (file != null)
                {
                    // Delete old photo if exists
                    DeletePhotoIfExists(photo.FilePath);

                    // Save new photo
                    photo.FilePath = await SavePhotoAsync(file);
                }

                await _roomRepository.UpdatePhotoAsync(photo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoomPhotoAsync(int photoId)
        {
            try
            {
                // Find the photo first
                var room = await _roomRepository.GetByIdWithDetailsAsync(0);
                var photo = room?.Photos.FirstOrDefault(p => p.Id == photoId);

                if (photo == null)
                {
                    return false;
                }

                // Delete the file
                DeletePhotoIfExists(photo.FilePath);

                // Delete from database
                await _roomRepository.DeletePhotoAsync(photoId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> SavePhotoAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, PhotosDirectory);

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return relative path from wwwroot
            return Path.Combine("/", PhotosDirectory, uniqueFileName).Replace("\\", "/");
        }

        private async Task SaveAdditionalPhotosAsync(Room room, List<IFormFile> files)
        {
            // Get highest display order to start with
            var startOrder = room.Photos.Any() ? room.Photos.Max(p => p.DisplayOrder) + 1 : 1;

            for (int i = 0; i < files.Count; i++)
            {
                var filePath = await SavePhotoAsync(files[i]);

                var photo = new RoomPhoto
                {
                    RoomId = room.Id,
                    FilePath = filePath,
                    DisplayOrder = startOrder + i,
                    IsFeatured = room.Photos.Count == 0 && i == 0 // First photo is featured if no others exist
                };

                await _roomRepository.AddPhotoAsync(photo);
            }
        }

        private void DeletePhotoIfExists(string? photoPath)
        {
            if (string.IsNullOrEmpty(photoPath))
            {
                return;
            }

            // Convert URL path to file system path
            var filePath = Path.Combine(_environment.WebRootPath, photoPath.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
