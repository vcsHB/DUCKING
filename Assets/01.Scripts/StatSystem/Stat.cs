using System.Collections.Generic;

namespace StatSystem
{
    [System.Serializable]
    public class Stat
    {
        public int baseValue;
        public List<int> modifiers;
        private int _changedValue;
        private bool _changed = true; 
        
        public void AddModifier(int value)
        {
            modifiers.Add(value);
            _changed = true;
        }

        public void RemoveModifier(int value)
        {
            modifiers.Remove(value);
            _changed = true;
        }

        public int GetValue()
        {
            if (_changed)
            {
                SetValueChange();
            }
            return _changedValue;
        }

        private void SetValueChange()
        {
            _changed = false;
            _changedValue = baseValue;
            for (int i = 0; i < modifiers.Count; i++)
            {
                _changedValue += modifiers[i];
            }
        }
        
    }
}