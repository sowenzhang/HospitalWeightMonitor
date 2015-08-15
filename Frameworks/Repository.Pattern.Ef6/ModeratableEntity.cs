using Repository.Pattern.Infrastructure;

namespace Repository.Pattern.Ef6
{
    public abstract class ModeratableEntity: Entity, IModerateStatus
    {
        public ModerateStatus ModerateStatus { get; set; }
    }
}