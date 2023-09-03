using WebApi.Repositories.LikeRepository;
using WebApi.Repositories.MessageRepository;
using WebApi.Repositories.UserRepository;

namespace WebApi.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    ILikeRepository LikeRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}
