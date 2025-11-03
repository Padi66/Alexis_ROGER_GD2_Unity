# BRASSART_GD2_Unity_RollABall

Systèmes de Gameplay Implémentés
1. Système de Déplacement du Joueur
Déplacement en 3D avec contrôles ZQSD
Système de saut avec détection du sol
Gravité personnalisée avec multiplicateurs de chute
Vitesse et force de saut modifiables
Gestion via Rigidbody et physique Unity
2. Système de Score
Système de score dynamique avec ScriptableObject (ScoreDatas)
Affichage en temps réel avec TextMeshPro
Évènements pour synchroniser le score entre systèmes
Conditions de victoire (score ≥ 10)
Conditions de défaite (score ≤ -1)
3. Système de Collectibles
Cibles 1 (TargetHard)
Donnent +1 point au score
Se régénèrent après 3 secondes
Collision basée sur OnCollisionEnter
Cibles 2 (TargetSoft)
Retirent 1 point au score
Disparaissent temporairement pendant 3 secondes
Collision basée sur trigger
Système de Spawn Dynamique
Génération aléatoire de cibles
Maximum de 5 cibles sur la carte simultanément
Respawn automatique quand une cible est collectée
Points de spawn multiples avec détection d'occupation
4. Système d'Inventaire et Cartes d'Accès
Gestion centralisée avec pattern Singleton
3 types de cartes d'accès configurables (AccessCardData)
Système de collection et retrait de cartes
Évènements pour notifier la collecte/utilisation
Interface utilisateur pour afficher le nombre de cartes
5. Système de Portes
Portes nécessitant des cartes d'accès spécifiques
Option pour consommer ou garder la carte après utilisation
Délai d'ouverture configurable
Feedback visuel (changement de couleur rouge→vert)
Feedback audio (sons de déverrouillage/refus)
6. Système de Boost
Zones qui augmentent temporairement la vitesse et le saut
Multiplicateurs configurables pour vitesse et saut
Durée de boost paramétrable (5 secondes par défaut)
Feedback visuel avec couleurs personnalisées
7. Système de Plaques de Pression
Activation au contact du joueur
Contrôle de murs temporaires qui disparaissent
Système de cooldown configurable (1 seconde)
Feedback visuel avec changement de matériau
Support pour contrôler plusieurs murs simultanément
8. Murs Dynamiques
Murs Disparaissants
Disparaissent pendant une durée définie (5 secondes)
Réapparaissent automatiquement
Désactivation visuelle et des collisions
Spawn Progressif de Murs
Génération de nouveaux murs quand une cible est collectée
Points de spawn prédéfinis
Arrêt automatique quand tous les points sont utilisés
9. Système de Mort et Respawn
Zone de mort (DeathBox) qui détecte la chute du joueur
Point de respawn (RespawnBox)
Réinitialisation des vélocités du Rigidbody
Téléportation instantanée au point de spawn
10. Système de Caméra
Caméra qui suit le joueur avec smoothing
Offset configurable
Rotation automatique vers le joueur
Zoom avec molette de souris (min: 3, max: 15)
Vitesse de zoom paramétrable
11. Interface Utilisateur (UI)
HUD de Jeu
Affichage du score en temps réel
Compteur de cartes d'accès collectées
Écran de Victoire
Panel qui s'affiche au score ≥ 10
Boutons "Redémarrer" et "Quitter"
Pause du jeu (Time.timeScale = 0)
Écran de Game Over
Panel qui s'affiche au score ≤ -1
Boutons "Redémarrer" et "Quitter"
Pause du jeu
12. Système de Menu Principal
Menu principal avec navigation entre panels
Panel d'options
Panel de crédits
Bouton "Jouer" pour lancer le jeu
Bouton "Quitter" avec support éditeur et build
