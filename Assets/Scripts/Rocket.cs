using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Transform    releasePos;
    [SerializeField] private GameObject   bulletPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float        bulletMass;
    [SerializeField] private float        forceSpeed;

    [SerializeField] private int maxLinePoint;

    private const float TimeBetweenPoint = 0.2f;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            this.FireBullet();
        var rotationZ = Input.GetAxis("Vertical") * 100f * Time.fixedDeltaTime;
        this.transform.Rotate(new Vector3(0, 0, rotationZ));
        this.DrawProjection();
    }

    private void FireBullet()
    {
        var bullet = Instantiate(this.bulletPrefab);
        bullet.transform.position = this.releasePos.position;
        var bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidBody.mass = this.bulletMass;
        bulletRigidBody.AddForce(this.transform.right * this.forceSpeed);
    }

    private void DrawProjection()
    {
        this.lineRenderer.enabled       = true;
        this.lineRenderer.positionCount = Mathf.CeilToInt(this.maxLinePoint / TimeBetweenPoint) + 1;
        var startVelocity = this.transform.right * this.forceSpeed / this.bulletMass;
        var startPosition = this.releasePos.position;
        var i             = 0;
        this.lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < this.maxLinePoint; time += TimeBetweenPoint)
        {
            i += 1;
            var point = startPosition + time * startVelocity;
            point.y = 0.5f * Physics.gravity.y * time * time + startVelocity.y * time + startPosition.y;
            this.lineRenderer.SetPosition(i, point);
        }
    }
}