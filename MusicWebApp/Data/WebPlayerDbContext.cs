using Microsoft.EntityFrameworkCore;
using MusicWebApp.Entities;

namespace MusicWebApp.Data
{
    public class WebPlayerDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Song>? Songs {  get; set; }
        public DbSet<Playlist>? Playlists {  get; set; }
        public DbSet<PlaylistToUser>? PlaylistsToUsers {  get; set; }
		public DbSet<SongToPlaylist>? SongsToPlaylists { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=localhost;Database=MusicDB;User Id=sa;Password=fmi");
        }
		
	}
}
