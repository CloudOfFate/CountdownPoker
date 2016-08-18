public enum Suits {Clubs,Diamonds,Hearts,Spades,NULL};

public enum Values {Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten,Jack,Queen,King,Ace,NULL};

public enum Hands {HighCard,OnePair,TwoPair,ThreeOfAKind,Straight,Flush,FullHouse,FourOfAKind,StraightFlush,RoyalFlush};

public enum TurnPhase {Initial,Betting,Auctioning,Swapping,Showdown,End};

public enum BettingDecision{Bet,Check,Call,Raise,Fold,NULL};

public enum AuctionPhase{Initial,First,Second,Third,End,NULL} //First Phase-> 5 cards for aunction Second Phase-> 3 cards for auction Last Phase -> Purchase of 1 random card

public enum AuctionDecision{Bid,Forfeit,NULL};

public enum AggressiveLevel{Defensive,Neutral,Aggressive};

public enum ValueOfHand{Low,Medium,High};

public enum ValueOfProjectedHand{Low,Medium,High};

public enum MoneyUsedForAction {Low,Medium,High};

public enum Desirability{Undesirable, Desirable, VeryDesirable};

public enum PlayerMoney{Low,Medium,High};

public enum Earning{Low,Medium,High};

public enum Tier{One,Two,Three,Four};

public enum RaiseType{RaiseToFold,RaiseToEarn};

public enum Rankings{Low,Medium,High};

public enum HandValueIncrease{Low,Medium,High};

public enum PlayerAction{PayLargeBind,PaySmallBind,BecomeDealer,UnderTheGun,Bet,Call,Check,Raise,Fold,Bid,Forfeit,Purchase,Sort,Win,Lost,SetMining,CheckRaise,Upswing,Downswing,Bluffing,SemiBluffing}

public enum Plays{SetMining,CheckRaise,Upswing,Downswing,Bluffing,SemiBluffing,NULL};

public enum EnemyMode{CROWD,RIVAL};

public enum HandGrade {Bad,Average,Good,NULL};

public enum TightnessLevel {Tight, Neutral, Loose};

public enum PositionLevel {Disadvantage, Neutral, Advantage};

public enum TablePosition {UnderTheGun,OnTheButton,SmallBind,BigBind};

public enum StackSizing {Short,Medium,Deep};

public enum ValidPlayerCount {Low,Medium,High};
