using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTower : Tower
{
    public int maxLasers = 7; // Maximum number of incoming lasers
    private float totalIntensity = 0f; // Combined laser intensity
    private float maxTotalIntensity = 200f; // Maximum combined intensity
    private float updateInterval = 0.1f; // Interval for updating laser intensity
    private float lastUpdateTime = 0f;

    private LaserManager laserManager;
    private Laser emittedLaser; // Laser emitted by this tower
    private Vector3 emittedDirection = Vector3.down; // Fixed downward emission direction

    public override void Initialize()
    {
        base.Initialize();
        laserManager = FindObjectOfType<LaserManager>();
    }

    public override void UpdateState()
    {
        // Update combined laser intensity at regular intervals
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateLaserIntensity();
            lastUpdateTime = Time.time;
        }
    }

    public override void ResetAttributes()
    {
        base.ResetAttributes();
        totalIntensity = 0f;
        lastUpdateTime = 0f;
        receivedLasers.Clear();

        // Remove emitted laser if present
        if (emittedLaser != null)
        {
            laserManager.RemoveLaser(this);
            emittedLaser = null;
        }
    }

    public override void OnLaserHit(Laser laser)
    {
        if (laser.sourceTower == this || receivedLasers.Contains(laser))
            return;

        // Ignore lasers nearly parallel to emitted direction
        if (Vector3.Dot(laser.direction, emittedDirection) > 0.9f) // 0.9 approximates parallel
            return;

        // Add laser if within max limit
        if (receivedLasers.Count < maxLasers)
            receivedLasers.Add(laser);

        // Create emitted laser on first received laser
        if (receivedLasers.Count == 1 && emittedLaser == null)
        {
            emittedLaser = laserManager.CreateLaser(this, transform.position, emittedDirection, 0); // Initial intensity set to 0
        }
    }

    public override void OnLaserOut(Laser laser)
    {
        if (receivedLasers.Contains(laser))
            receivedLasers.Remove(laser);

        // Remove emitted laser if no incoming lasers remain
        if (receivedLasers.Count == 0 && emittedLaser != null)
        {
            laserManager.RemoveLaser(this);
            emittedLaser = null;
        }
    }

    public void UpdateLaserIntensity()
    {
        // Calculate total incoming intensity
        totalIntensity = 0f;
        foreach (var receivedLaser in receivedLasers)
            totalIntensity += receivedLaser.intensity;

        // Cap total intensity to max allowed
        totalIntensity = Mathf.Min(totalIntensity, maxTotalIntensity);

        // Update emitted laser intensity
        if (emittedLaser != null)
        {
            emittedLaser.SetLaserProperties(totalIntensity, emittedDirection);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle incoming laser collision
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
                OnLaserHit(laser);
        }
    }
    
    protected void OnTriggerExit2D(Collider2D collision)
    {
        // Handle outgoing laser collision
        if (collision.CompareTag("Laser"))
        {
            Laser laser = collision.GetComponent<Laser>();
            if (laser != null)
                OnLaserOut(laser);
        }
    }
}