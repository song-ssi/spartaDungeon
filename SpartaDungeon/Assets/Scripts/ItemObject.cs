using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IInteractable
    {
        public string GetInteractPrompt();
        public void OnInteract();
    }

// 프롬프트에 아이템 정보 출력, 아이템 획득
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
