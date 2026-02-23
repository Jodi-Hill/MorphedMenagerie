namespace RaafOritme.SmartNPCs
{
    public enum BrainType
    {
        FSM,
        GOAP
    }

    public enum OverRuleState
    {
        NONE,
        COMBAT,
        SCARED,
        ANGRY,
        DECEASED,
        CHATTING,
    }

    [System.Flags]
    public enum Modules
    {
        PATROL = 1 << 0,
        IDLE = 1 << 1,
        COMBAT = 1 << 2,
        DIALOGUE = 1 << 3,
        SENSORY = 1 << 4,
    }

    public enum AnimationType
    {
        NONE,
        SLEEPING,
        SITTING,
        EATING,
        DRINKING,
        LISTENING,
        CHATTING,
        INSPECTING,
        TRADING,
        OBSERVING,
    }

    public enum IdleState
    {
        NONE,
        RESTING,
        NAVIGATING,
    }

    public enum Vitality
    {
        EXTREMELY_STRONG = 0,
        VERY_STRONG = 1,
        STRONG = 2,
        MODERATE = 3,
        WEAK = 4,
        VERY_WEAK = 5,
        EXTREMELY_WEAK = 6,
    }

    public enum EmotionState
    {
        Neutral,
        Angry,
        Happy,
        Sad,
    }

    public enum ActionType
    {
        Aggressive,
        Passive,
        Social,
    }
}
