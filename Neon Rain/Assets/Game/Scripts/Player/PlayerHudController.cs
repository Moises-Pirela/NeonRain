using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHudController : MonoBehaviour
{
    public enum WeaponType
    {
        PISTOL,
        GRAVITY_ATTRACTOR,
        GRAVITY_GRAPPLE
    }

    public static Action onCloseWeaponWheel;
    
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;
    
    [SerializeField] private Button pistolButton;
    [SerializeField] private Button gravityGrappleButton;
    [SerializeField] private Button gravityAttractorButton;
    [SerializeField] private Button daggerButton;
    [SerializeField] private Button magenetizeButton;

    [SerializeField] private GameObject deathScreen;

    [SerializeField] private BaseEntity player;

    [SerializeField] private GameObject weaponWheel;
    [SerializeField] private GameObject weaponBar;
    [SerializeField] private GameObject[] weaponSlotsHighlight;

    [SerializeField] private TextMeshProUGUI toolTip;

    [SerializeField] private Image restImage;

    private PlayerControls _playerControls;

    [SerializeField] private InteractionController _interactionController;

    [SerializeField] private Animator _animator;

    private bool changedWeapon;

    public static bool ChangedWeapon;

    private bool changingWeapons;

    private void Awake()
    {
        player.onArmorChange += UpdateShieldStatusBar;
        player.onHealthChange += UpdateHealthStatusBar;
        player.onDeath += DeathAnimation;
        PlayerEvents.Current.onRest += RestAnimation;
        GameMaster._current.onRestartLevel += () =>
        {
            _animator.ResetTrigger("Die");
            _animator.SetTrigger("Restart");
            deathScreen.SetActive(false);
        };
        
        _playerControls = new PlayerControls();
    }

    private void Update()
    {
        toolTip.text = string.Empty;
        
        if (!_interactionController._currentInteractable) return;

        toolTip.text = _interactionController._currentInteractable.data.toolTip;
    }

    private void Start()
    {
        _playerControls.Player.WeaponWheel.performed += context => { ShowWeaponWheel(); };
        _playerControls.Player.WeaponWheel.canceled += context => { CloseWeaponWheel(); };
        _playerControls.Player.Equipment1.performed += context =>
        {
            StartCoroutine(ShowWeaponBar(0));
        };
        _playerControls.Player.Equipment2.performed += context =>
        {
            StartCoroutine(ShowWeaponBar(1));
        };
        _playerControls.Player.Equipment3.performed += context =>
        {
            StartCoroutine(ShowWeaponBar(2));
        };
        _playerControls.Player.Equipment4.performed += context =>
        {
            StartCoroutine(ShowWeaponBar(3));
        };
        _playerControls.Player.Equipment5.performed += context =>
        {
            StartCoroutine(ShowWeaponBar(4));
        };
    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private IEnumerator ShowWeaponBar(int weaponIndex)
    {
        weaponSlotsHighlight[weaponIndex].SetActive(true);
        
        if(changingWeapons)
            yield break;
        
        changingWeapons = true;
        
        weaponBar.transform.DOMoveY(0, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);
        
        HideWeaponBar();
    }

    private void HideWeaponBar()
    {
        weaponBar.transform.DOMoveY(-265f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            foreach (var o in weaponSlotsHighlight)
            {
                o.SetActive(false);
                changingWeapons = false;
            }    
        });
    }
    
    private void ShowWeaponWheel()
    {
        weaponWheel.gameObject.SetActive(true);
        Time.timeScale = 0.1f;
        GameMaster._current.IsPaused = true;
        // pistolButton.interactable = SaveData.Current.inventory.pistol.IsUnlocked;
        // gravityAttractorButton.interactable = SaveData.Current.inventory.gravityAttractor.IsUnlocked;
        // gravityGrappleButton.interactable = SaveData.Current.inventory.gravityGrapple.IsUnlocked;
        // daggerButton.interactable = SaveData.Current.inventory.dagger.IsUnlocked;
        // magenetizeButton.interactable = SaveData.Current.inventory.dagger.IsUnlocked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void CloseWeaponWheel()
    {
        weaponWheel.gameObject.SetActive(false);
        changedWeapon = ChangedWeapon;
        onCloseWeaponWheel?.Invoke();
        
        Time.timeScale = 1;
        GameMaster._current.IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        changedWeapon = false;
        ChangedWeapon = changedWeapon;
    }

    private void DeathAnimation()
    {
        _animator.SetTrigger("Die");
    }

    private void UpdateShieldStatusBar(float armor, float initialArmor)
    {
        shieldSlider.value = armor / player.maxShield;
    }
    private void UpdateHealthStatusBar()
    {
        healthSlider.value = player.Health / player.maxHealth;
    }

    private void RestAnimation()
    {
        GameMaster._current.IsPaused = true;
        
        restImage.gameObject.SetActive(true);
        
        restImage.DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() =>
        {
            restImage.DOColor(new Color(0, 0, 0, 0), 1f).OnComplete(() =>
            {
                GameMaster._current.IsPaused = false;
                restImage.gameObject.SetActive(false);
            });
        });
    }
}
