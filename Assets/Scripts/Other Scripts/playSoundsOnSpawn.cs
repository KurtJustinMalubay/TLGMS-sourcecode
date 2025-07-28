using UnityEngine;

public class playSoundsOnSpawn : MonoBehaviour
{
    public AudioSource[] spawnAudio;
    public spawnChunk spawn; // Make sure this is assigned properly
    private float timer;
    private int randomizer;
    private bool isPlaying = false;

    private void Update()
    {
        if (isPlaying)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spawnAudio[randomizer].Stop();
                isPlaying = false;
            }
        }
    }

    public void PlaySounds()
    {
        if (spawnAudio.Length == 0) return;
        randomizer = Random.Range(0, spawnAudio.Length);
        spawnAudio[randomizer].Play();
        timer = spawnAudio[randomizer].clip.length;
        isPlaying = true;
    }
}
