# ⛳ cutline

a fantasy golf league app built around one simple idea:

**your players' finishing positions are your points. lowest score wins.**

that's it.

no salaries.  
no betting.  
no stroke-by-stroke math.  
no "projected ownership percentage."

just draft golfers, survive the cut, and hope your picks don't implode on sunday.

## 🏌️ the format

- draft a team of golfers
- each golfer earns points equal to their finishing position
  - 1st place = 1 point
  - 34th place = 34 points
  - missed the cut? points = the number of players made the cut.
- scores are aggregated across tournaments
- lowest total score wins

simple. brutal. beautiful.

## 🎯 why this exists

a group of friends heard about this game format.

this project is just a way to:

- handle drafts cleanly
- automate the scoring
- track standings across tournaments
- build something fun in the process

it's a side project.  
it's not a business.  
it's not trying to replace official fantasy platforms.

it's just cutline.

## 🧠 important note

i am **not the original creator of this fantasy format**.  
this repository is simply my own implementation of the concept and something practical for our league.

game formats evolve organically. this is just one implementation of a format we thought of playing.

## 🛠 tech stack

cutline is built with:

- **backend:** (.net)
- **database:** relational (sql-based)
- **frontend:** tbd
- **authentication:** tbd
- **hosting:** designed to be self-hostable

the backend focuses on a clean and understandable domain model:

- leagues
- league members
- draft picks
- tournaments
- player results
- aggregate scoring

## 🖥 self-host friendly by design

cutline is intended to be easy to run yourself.

the ambition is:

- minimal infrastructure requirements
- one deployable service
- one relational database
- simple configuration via environment variables

if you want to run this for your own league, you should be able to do.

## 🚧 status

active side project.  
built for small private leagues.  
may evolve. may not. that's part of the fun.

## 📜 license

mit license — use it, fork it, improve it.  
just don't blame me if your first-round pick misses the cut.
