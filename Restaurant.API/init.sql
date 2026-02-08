CREATE TABLE "Consumers" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "CreatedAtUtc" timestamptz NOT NULL,
    "UpdatedAtUtc" timestamptz NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAtUtc" timestamptz,
    CONSTRAINT "PK_Consumers" PRIMARY KEY ("Id")
);


CREATE TABLE "MenuCategories" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" varchar NOT NULL,
    "CreatedAtUtc" timestamptz NOT NULL,
    "UpdatedAtUtc" timestamptz NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAtUtc" timestamptz,
    CONSTRAINT "PK_MenuCategories" PRIMARY KEY ("Id")
);


CREATE TABLE "Orders" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "ConsumerId" uuid,
    "Status" varchar NOT NULL,
    "CreatedAtUtc" timestamptz NOT NULL,
    "UpdatedAtUtc" timestamptz NOT NULL,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Orders_Consumers_ConsumerId" FOREIGN KEY ("ConsumerId") REFERENCES "Consumers" ("Id")
);


CREATE TABLE "MenuItems" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" varchar NOT NULL,
    "Description" text NOT NULL,
    "ImageUrl" varchar NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "CategoryId" uuid NOT NULL,
    "CreatedAtUtc" timestamptz NOT NULL,
    "UpdatedAtUtc" timestamptz NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "DeletedAtUtc" timestamptz,
    CONSTRAINT "PK_MenuItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MenuItems_MenuCategories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "MenuCategories" ("Id") ON DELETE CASCADE
);


CREATE TABLE "OrderLineItems" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "MenuItemId" uuid NOT NULL,
    "Count" integer NOT NULL,
    "OrderId" uuid,
    CONSTRAINT "PK_OrderLineItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OrderLineItems_MenuItems_MenuItemId" FOREIGN KEY ("MenuItemId") REFERENCES "MenuItems" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderLineItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id")
);


INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('04a8f982-b59f-41a8-b119-db5acdf96700', TIMESTAMPTZ '2026-02-08T20:56:13.44204+00:00', NULL, FALSE, 'Drinks', TIMESTAMPTZ '2026-02-08T20:56:13.442076+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('3f468941-50ae-4bdf-aea4-47fdfd4cbb1e', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00', NULL, FALSE, 'Steaks', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('50c1ab12-22f6-4c9e-8895-dd997e6286dd', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00', NULL, FALSE, 'Barbecue', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('7c9883c4-a24a-4403-8588-5972ba14a90e', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00', NULL, FALSE, 'Desserts', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('7cff41a6-125b-44c9-8a02-98a3808885fb', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00', NULL, FALSE, 'Coffee', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('a3f97e6b-6fe9-4c52-bfe7-29fcedf10246', TIMESTAMPTZ '2026-02-08T20:56:13.442108+00:00', NULL, FALSE, 'Hot Dogs', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('c9c2d401-12d9-40ca-befa-2ffc70449e30', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00', NULL, FALSE, 'Seafood', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('fbb342e6-910e-493c-93c7-5b1b58f69cd7', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00', NULL, FALSE, 'Fast Food', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00');
INSERT INTO "MenuCategories" ("Id", "CreatedAtUtc", "DeletedAtUtc", "IsDeleted", "Name", "UpdatedAtUtc")
VALUES ('fc32339e-b6cc-44e0-80cb-f0e38fc12d6b', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00', NULL, FALSE, 'Pizzas', TIMESTAMPTZ '2026-02-08T20:56:13.442109+00:00');


CREATE UNIQUE INDEX "IX_MenuCategories_Name" ON "MenuCategories" ("Name");


CREATE INDEX "IX_MenuItems_CategoryId" ON "MenuItems" ("CategoryId");


CREATE UNIQUE INDEX "IX_MenuItems_Name" ON "MenuItems" ("Name");


CREATE INDEX "IX_OrderLineItems_MenuItemId" ON "OrderLineItems" ("MenuItemId");


CREATE INDEX "IX_OrderLineItems_OrderId" ON "OrderLineItems" ("OrderId");


CREATE INDEX "IX_Orders_ConsumerId" ON "Orders" ("ConsumerId");


