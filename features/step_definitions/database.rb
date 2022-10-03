# frozen_string_literal: true

require 'sqlite3'

Then('we can open a SQLite connection to {string}') do |database|
  database = resolve_path database
  db = SQLite3::Database.open database
  r = db.execute('PRAGMA integrity_check')
  expect(r[0]).to match(['ok'])
end

Then('the {channel} should contain the version of {string}') do |_channel, dll|
  command_output = `dotnet dll-props #{dll}`
  matches = /ProductVersion: (.*)/.match command_output
  expect(last_command_started).to have_output output_string_eq matches[1]
end

Then('the {string} contains history interval stop at {string} {string}') do |database, commit_date, commit_id|
  database = resolve_path database
  db = SQLite3::Database.open database
  r = db.execute("SELECT GitCommitDate, GitCommitId FROM CachedHistoryIntervalStops WHERE GitCommitId='#{commit_id}' AND GitCommitDate LIKE '#{commit_date}%'")
  expect(r[0][0]).to match(/#{commit_date}/)
  expect(r[0][1]).to match(commit_id)
end
