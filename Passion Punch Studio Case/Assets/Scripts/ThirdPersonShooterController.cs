using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using TMPro;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private float deviationRate;
    [SerializeField] private int minNumberOfBullets;
    [SerializeField] private int maxNumberOfBullets;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshPro totalNumberOfBulletstText;
    [SerializeField] private Transform explodingBullet;
    [SerializeField] private Transform biggerBullet;
    [SerializeField] private Transform normalBullet;
    [SerializeField] private Transform redBullet;


    private Transform bulletPrefab;
    public int numberOfBullets;
    private int totalNumberOfBullets = 0;
    private Animator animator;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        bulletPrefab = normalBullet;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bulletPrefab = normalBullet;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bulletPrefab = biggerBullet;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bulletPrefab = explodingBullet;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            bulletPrefab = redBullet;
        }
        totalNumberOfBulletstText.text = totalNumberOfBullets.ToString();
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }
        if (starterAssetsInputs.aim)
        {
            crosshair.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            if (starterAssetsInputs.shoot)
            {
                numberOfBullets = Random.Range(minNumberOfBullets, maxNumberOfBullets);
                totalNumberOfBullets += numberOfBullets;
                for (int i = 0; i < numberOfBullets; i++)
                {
                    var randomNumberX = Random.Range(-deviationRate, deviationRate);
                    var randomNumberY = Random.Range(-deviationRate, deviationRate);
                    var randomNumberZ = Random.Range(-deviationRate, deviationRate);
                    Vector3 randomPos = new Vector3(randomNumberX, randomNumberY, randomNumberZ);
                    Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

                    Instantiate(bulletPrefab, spawnBulletPosition.position, Quaternion.LookRotation(aimDir + randomPos, Vector3.up));
                    starterAssetsInputs.shoot = false;
                }

            }
        }
        else
        {
            crosshair.SetActive(false);
            starterAssetsInputs.shoot = false;
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

       

        
        
    }
}
