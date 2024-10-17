using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapAndPokeObject : MonoBehaviour
{
    public InteractableUnityEventWrapper handGrabInteractable;
    public InteractableUnityEventWrapper pokeInteractable;

    [SerializeField] private GrapBlock _grapBlock;
    public event Action<GrapAndPokeObject, SheetBase, int> OnGrapReleasedAddBlock;
    
    private void OnEnable()
    {
        // HandGrab과 Poke 이벤트를 구독
        handGrabInteractable.WhenSelect.AddListener(OnGrabbed);
        handGrabInteractable.WhenUnselect.AddListener(OnGrabReleased);
        pokeInteractable.WhenSelect.AddListener(OnPoked);
        pokeInteractable.WhenUnselect.AddListener(OnPokeReleased);
    }
    private void OnDisable()
    {
        // HandGrab과 Poke 이벤트 구독 해제
        handGrabInteractable.WhenSelect.RemoveListener(OnGrabbed);
        handGrabInteractable.WhenUnselect.RemoveListener(OnGrabReleased);
        pokeInteractable.WhenSelect.RemoveListener(OnPoked);
        pokeInteractable.WhenUnselect.RemoveListener(OnPokeReleased);
    }
    private void OnGrabbed()
    {
        // HandGrab이 활성화되면 Poke 비활성화
        pokeInteractable.enabled = false;
        Debug.Log("handGrab ON");
    }
    private void OnGrabReleased()
    {
        // HandGrab이 해제되면 Poke 다시 활성화
        OnGrapReleasedAddBlock?.Invoke(this, _grapBlock.TargetSheet, _grapBlock.BlockIndex);
        
        StartCoroutine(Coroutine_ResetPosition());
        Debug.Log("handGrab OFF");
    }
    private void OnPoked()
    {
        // Poke가 활성화되면 HandGrab 비활성화
        handGrabInteractable.enabled = false;
        handGrabInteractable.gameObject.SetActive(false);
        Debug.Log("Poke ON");
    }
    private void OnPokeReleased()
    {
        // Poke가 해제되면 HandGrab 다시 활성화
        handGrabInteractable.enabled = true;
        handGrabInteractable.gameObject.SetActive(true);
        Debug.Log("Poke OFF");
    }

    IEnumerator Coroutine_ResetPosition()
    {
        yield return new WaitForEndOfFrame();
        
        pokeInteractable.enabled = true;
        handGrabInteractable.transform.localPosition = Vector3.zero;
        handGrabInteractable.transform.localRotation = Quaternion.identity;
    }
}
