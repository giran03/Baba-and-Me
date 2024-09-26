public class EnemyStateFactory
{
    EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyBaseState EnemyAttack()
    {
        return new EnemyAttack(_context, this);
    }

    public EnemyBaseState EnemyChase()
    {
        return new EnemyChase(_context, this);
    }

    public EnemyBaseState EnemyPatrol()
    {
        return new EnemyPatrol(_context, this);
    }
}
