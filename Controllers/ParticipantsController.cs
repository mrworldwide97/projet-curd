using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class ParticipantsController : ControllerBase
{
    // Liste simulant une base de données en mémoire
    private static List<string> participants = new List<string>
    {
        "Participant1",
        "Participant2"
    };

    // Méthode GET pour récupérer tous les participants
    [HttpGet]
    public IActionResult GetParticipants()
    {
        return Ok(participants);
    }

    // Méthode GET pour récupérer un participant par ID
    [HttpGet("{id}")]
    public IActionResult GetParticipant(int id)
    {
        if (id < 0 || id >= participants.Count)
        {
            return NotFound("Participant non trouvé.");
        }

        return Ok(participants[id]);
    }

    // Méthode POST pour ajouter un nouveau participant
    [HttpPost]
    public IActionResult AddParticipant([FromBody] string participant)
    {
        if (string.IsNullOrWhiteSpace(participant))
        {
            return BadRequest("Le nom du participant ne peut pas être vide.");
        }

        participants.Add(participant);
        return CreatedAtAction(nameof(GetParticipant), new { id = participants.Count - 1 }, participant);
    }

    // Méthode PUT pour mettre à jour un participant existant
    [HttpPut("{id}")]
    public IActionResult UpdateParticipant(int id, [FromBody] string participant)
    {
        if (id < 0 || id >= participants.Count)
        {
            return NotFound("Participant non trouvé.");
        }

        if (string.IsNullOrWhiteSpace(participant))
        {
            return BadRequest("Le nom du participant ne peut pas être vide.");
        }

        participants[id] = participant;
        return NoContent(); // 204 No Content
    }

    // Méthode DELETE pour supprimer un participant
    [HttpDelete("{id}")]
    public IActionResult DeleteParticipant(int id)
    {
        if (id < 0 || id >= participants.Count)
        {
            return NotFound("Participant non trouvé.");
        }

        participants.RemoveAt(id);
        return NoContent(); // 204 No Content
    }
}
