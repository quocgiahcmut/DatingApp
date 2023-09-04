using AutoMapper;
using WebApi.Data;
using WebApi.Repositories.LikeRepository;
using WebApi.Repositories.MessageRepository;
using WebApi.Repositories.PhotoRepo;
using WebApi.Repositories.UserRepository;

using LikeRepo = WebApi.Repositories.LikeRepository;
using MessageRepo = WebApi.Repositories.MessageRepository;
using UserRepo = WebApi.Repositories.UserRepository;

namespace WebApi.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;

    public UnitOfWork(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IUserRepository UserRepository => new UserRepo.UserRepository(_context, _mapper);

    public IMessageRepository MessageRepository => new MessageRepo.MessageRepository(_context, _mapper);

    public ILikeRepository LikeRepository => new LikeRepo.LikeRepository(_context);

    public IPhotoRepository PhotoRepository => new PhotoRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
