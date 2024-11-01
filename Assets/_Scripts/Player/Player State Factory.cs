public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdle(_context, this);
    }

    public PlayerBaseState Walking()
    {
        return new PlayerWalking(_context, this);
    }

    public PlayerBaseState Hurt()
    {
        return new PlayerHurt(_context, this);
    }

    public PlayerBaseState Attacking()
    {
        return new PlayerAttacking(_context, this);
    }
}