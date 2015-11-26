---
aliases:
 - /hacker-vandalism
author: "Johan Idstam"
date: 2012-10-25
title: Hacker vandalism
wordpress_id: 208
categories:
- Status
---

It has been a hectic couple of weeks trying to keep ahead of the script kiddie who has been vandalizing my sites.

I'm still not sure if it is one of my sites or Dreamhost as a whole that has been compromised but my sites there are trashed within minutes after I fix them.

A part from the vulnerability in it self the main factor for my troubles are that Dreamhost is running Apache under my user account. Since I have permission to alter all my files so does Apache.

A simple directory traversal will give an attacker access to all my sites under the compromised account.

I've now moved most of the attacked sites to a VPS where I have better control. Considering all time I've spent cleaning up after attacks I think it will be cheaper to admin a whole server.
