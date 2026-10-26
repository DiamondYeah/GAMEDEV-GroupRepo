using UnityEngine;

public class SuckBullet : MonoBehaviour
{
    public float Damage;
    public float BulletRange;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform gunMuzzle;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot()
    {
        Ray gunRay = new Ray(playerCamera.position, playerCamera.forward);
        Vector3 target;
        if(Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            target = hitInfo.point;
        }
        else
        {
            target = gunRay.origin + gunRay.direction * BulletRange;
        }
        GameObject newBullet = Instantiate(bullet, gunMuzzle.position, gunMuzzle.rotation);
        Vector3 dir = (target - gunMuzzle.position).normalized;
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.AddForce(dir * 50f, ForceMode.Impulse);
        
    }
}
