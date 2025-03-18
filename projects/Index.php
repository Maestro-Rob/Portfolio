<?php
$pdo = new PDO("mysql:host=localhost;dbname=portfolio;charset=utf8", "root", ""); // Passe Benutzer/Passwort an
$projects = $pdo->query("SELECT * FROM projects")->fetchAll(PDO::FETCH_ASSOC);
?>
<!DOCTYPE html>
<html lang="de">
<head>
  <meta charset="UTF-8">
  <title>GameDev Portfolio</title>
  <link rel="stylesheet" href="style.css">
</head>
<body>

  <header>
    <h1>🎮 Mein GameDev-Portfolio</h1>
    <p>Spiele, Prototypen & Tools von Robin Honeck</p>
  </header>

  <section class="projects">
    <h2>🧩 Meine Projekte</h2>
    <div class="grid">
      <?php foreach ($projects as $p): ?>
        <div class="card">
          <img src="<?= htmlspecialchars($p['image_url']) ?>" alt="<?= htmlspecialchars($p['title']) ?>">
          <h3><?= htmlspecialchars($p['title']) ?></h3>
          <p><?= htmlspecialchars($p['description']) ?></p>
          <p><a href="<?= htmlspecialchars($project['project_url']) ?>" target="_blank">🔗 Projekt ansehen>🎮 Demo </a></p>
          
        </div>
      <?php endforeach; ?>
    </div>
  </section>

  <section class="skills">
    <h2>🧠 Tech-Stack & Skills</h2>
    <ul>
      <li>Sprachen: JavaScript, C#, HTML, CSS</li>
      <li>Engines/Frameworks: Unity, Phaser.js, Three.js</li>
      <li>Tools: Git, VS Code, Unity, Blender, Photoshop</li>
    </ul>
  </section>

  <section class="about">
    <h2>🙋‍♂️ Über mich</h2>
    <p>Hi! Ich bin ein begeisterter Game-Developer. Ich liebe es, Welten zu erschaffen, Mechaniken auszuprobieren und neue Tools zu entwickeln. Ziel: in einem Game-Studio oder Indie-Team durchstarten!</p>
  </section>

  <section class="contact">
    <h2>📞 Kontakt</h2>
    <p>E-Mail: <a href="mailto:2023gmehonrob@gmail.com">2023gmehonrob@gmail.com</a></p>
    <p>Telefon: <a href="">+49 0178 1328114</a></p>
    <p>LinkedIn: <a href="https://www.linkedin.com/in/robin-honeck-76877433b/" target="_blank">Profil ansehen</a></p>
    <p>Discord: maestro_rob</p>
  </section>

</body>
</html>