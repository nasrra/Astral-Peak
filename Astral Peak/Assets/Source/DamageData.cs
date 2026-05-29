[System.Serializable]
public class DamageData{
    public DamageData(int _damage, bool _unblockable = false){
        damage              = _damage;
        unblockable    = _unblockable;
    }
    
    public int 
        damage;
    public bool unblockable = false;
}
