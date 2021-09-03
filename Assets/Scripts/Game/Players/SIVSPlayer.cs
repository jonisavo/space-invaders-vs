using UnityEngine;

namespace SIVS
{
    public class SIVSPlayer
    {
        public const int InitialLives = 3;
        public const int MaxLives = 5;
        
        public string Name { get; }
        
        public int Number { get; }

        public virtual bool Ready
        {
            get => _ready;
            set
            {
                _ready = value;
                EmitReadyChangeEvent(value);
            }
        }

        private bool _ready = false;

        public virtual int CurrentRound
        {
            get => _currentRound;
            set
            {
                _currentRound = value;
                EmitRoundChangeEvent(value);
            }
        }

        private int _currentRound;

        public virtual int Score
        {
            get => _score;
            set
            {
                _score = value;
                EmitScoreChangeEvent(value);
            }
        }

        private int _score;

        public virtual int InvaderKills
        {
            get => _invaderKills;
            set
            {
                _invaderKills = value;
                EmitInvaderKillsChangeEvent(value);
            }
        }

        private int _invaderKills;

        public virtual int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                EmitLivesChangeEvent(value);
            }
        }

        private int _lives = InitialLives;

        public virtual PlayerBulletType BulletType
        {
            get => _bulletType;
            set
            {
                _bulletType = value;
                EmitBulletTypeChangeEvent(value);
            }
        }

        private PlayerBulletType _bulletType = PlayerBulletType.Normal;
        
        public delegate void ReadyChangeDelegate(SIVSPlayer player, bool isReady);

        public static event ReadyChangeDelegate OnReadyChange;

        public delegate void ScoreChangeDelegate(SIVSPlayer player, int newScore);

        public static event ScoreChangeDelegate OnScoreChange;

        public delegate void RoundChangeDelegate(SIVSPlayer player, int newRound);

        public static event RoundChangeDelegate OnRoundChange;

        public delegate void LivesChangeDelegate(SIVSPlayer player, int newLives);

        public static event LivesChangeDelegate OnLivesChange;
        
        public delegate void InvaderKillsChangeDelegate(SIVSPlayer player, int newKills);

        public static event InvaderKillsChangeDelegate OnInvaderKillsChange;
        
        public delegate void BulletTypeChange(SIVSPlayer player, PlayerBulletType newBulletType);

        public static event BulletTypeChange OnBulletTypeChange;

        public SIVSPlayer(string name, int number)
        {
            Name = name;
            Number = number;
        }
        
        public virtual void Cleanup() {}

        public virtual void InitializeStats()
        {
            CurrentRound = 1;
            Lives = InitialLives;
            Score = 0;
            InvaderKills = 0;
            BulletType = PlayerBulletType.Normal;
            Ready = true;
        }

        public virtual void AddScore(int amount)
        {
            Score += amount;
        }

        public virtual void GoToNextRound()
        {
            CurrentRound += 1;
        }

        public virtual void AddLife()
        {
            Lives = Mathf.Clamp(Lives + 1, 0, MaxLives);
        }

        public virtual void RemoveLife()
        {
            Lives = Mathf.Clamp(Lives - 1, 0, MaxLives);
        }

        protected void EmitLivesChangeEvent(int newLives) =>
            OnLivesChange?.Invoke(this, newLives);
        
        protected void EmitScoreChangeEvent(int newScore) =>
            OnScoreChange?.Invoke(this, newScore);
        
        protected void EmitReadyChangeEvent(bool isReady) =>
            OnReadyChange?.Invoke(this, isReady);

        protected void EmitRoundChangeEvent(int newRound) =>
            OnRoundChange?.Invoke(this, newRound);
        
        protected void EmitInvaderKillsChangeEvent(int newKills) =>
            OnInvaderKillsChange?.Invoke(this, newKills);
        
        protected void EmitBulletTypeChangeEvent(PlayerBulletType newBulletType) =>
            OnBulletTypeChange?.Invoke(this, newBulletType);

        public override string ToString() => $"{Name} ({Number})";
    }
}