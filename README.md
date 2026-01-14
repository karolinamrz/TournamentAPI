Instrukcja: 

1. Rejestracja graczy:
   
mutation {
  p1: registerUser(firstName: "Jan", lastName: "Kowalski", email: "jan@test.com", password: "123")
  p2: registerUser(firstName: "Anna", lastName: "Nowak", email: "anna@test.com", password: "123")
}

2.Logowanie i pobieranie tokena

 mutation {
 loginUser(email: "jan@test.com",  password: "123")
}

Skopiuj token z odpowiedzi

3. Tworzenie turnieju:

   mutation {
  createTournament(name: "Mistrzostwa IT") {
    id
    name
  }
}

Zapisz ID turnieju z odpowiedzi

4. Dodawanie uczetnikow:
   
 mutation {
  add1: addParticipant(tournamentId: 1, userId: 1)
  add2: addParticipant(tournamentId: 1, userId: 2)
}

6. Rozpoczecie turnieju:
   
   mutation {
  startTournament(tournamentId: 1)
}

System automatycznie generuje drabinkę turniejową

6. Podgląd swoich meczów:
   
   query {
  myMatches {
    id
    round
    player1 { firstName }
    player2 { firstName }
    winner { firstName }
  }
}

Jeśli widzisz błąd "nie masz uprawnień", sprawdź czy jesteś zalogowany

7. Rozegranie meczu
   
mutation {
  playMatch(matchId: 1, winnerId: 1) {
    id
    winner { firstName }
  }
}

9. Podgląd drabinki turniejowej (nie wymaga logowania)
    
query {
  tournaments(where: { id: { eq: 1 } }) {
    name
    bracket {
      matches {
        id
        round
        player1 { firstName }
        player2 { firstName }
        winner { firstName }
      }
    }
  }
}

11. Funkcjonalności

Rejestracja i logowanie

Tworzenie turniejów pucharowych

Automatyczne generowanie drabinki

Rozgrywanie meczów

Pobieranie swoich meczów (po zalogowaniu)

