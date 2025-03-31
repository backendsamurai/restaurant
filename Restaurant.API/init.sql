CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
CREATE TABLE customers (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name text NOT NULL,
    email text NOT NULL,
    password_hash text NOT NULL,
    CONSTRAINT pk_customers PRIMARY KEY (id)
);

CREATE TABLE product_categories (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    CONSTRAINT pk_product_categories PRIMARY KEY (id)
);

CREATE TABLE orders (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    customer_id uuid NOT NULL,
    status varchar NOT NULL,
    created_at timestamp NOT NULL DEFAULT (now()),
    updated_at timestamp NOT NULL DEFAULT (now()),
    CONSTRAINT pk_orders PRIMARY KEY (id),
    CONSTRAINT fk_orders_customers_customer_id FOREIGN KEY (customer_id) REFERENCES customers (id) ON DELETE CASCADE
);

CREATE TABLE products (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    description text NOT NULL,
    image_url varchar NOT NULL,
    price numeric(18,2) NOT NULL,
    category_id uuid NOT NULL,
    CONSTRAINT pk_products PRIMARY KEY (id),
    CONSTRAINT fk_products_product_categories_category_id FOREIGN KEY (category_id) REFERENCES product_categories (id) ON DELETE CASCADE
);

CREATE TABLE payments (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    order_id uuid NOT NULL,
    bill numeric(18,2) NOT NULL,
    tip numeric(18,2),
    created_at timestamp NOT NULL DEFAULT (now()),
    updated_at timestamp NOT NULL DEFAULT (now()),
    CONSTRAINT pk_payments PRIMARY KEY (id),
    CONSTRAINT fk_payments_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
);

CREATE TABLE order_line_items (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    product_id uuid NOT NULL,
    count integer NOT NULL,
    order_id uuid,
    CONSTRAINT pk_order_line_items PRIMARY KEY (id),
    CONSTRAINT fk_order_line_items_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id),
    CONSTRAINT fk_order_line_items_products_product_id FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

INSERT INTO product_categories (id, name)
VALUES ('04a8f982-b59f-41a8-b119-db5acdf96700', 'Sushi');
INSERT INTO product_categories (id, name)
VALUES ('28a273f6-53b2-4815-b00c-41473b9cb346', 'Drinks');
INSERT INTO product_categories (id, name)
VALUES ('3f468941-50ae-4bdf-aea4-47fdfd4cbb1e', 'Steaks');
INSERT INTO product_categories (id, name)
VALUES ('50c1ab12-22f6-4c9e-8895-dd997e6286dd', 'Barbecue');
INSERT INTO product_categories (id, name)
VALUES ('7c9883c4-a24a-4403-8588-5972ba14a90e', 'Desserts');
INSERT INTO product_categories (id, name)
VALUES ('7cff41a6-125b-44c9-8a02-98a3808885fb', 'Coffee');
INSERT INTO product_categories (id, name)
VALUES ('a3f97e6b-6fe9-4c52-bfe7-29fcedf10246', 'Hot Dogs');
INSERT INTO product_categories (id, name)
VALUES ('c9c2d401-12d9-40ca-befa-2ffc70449e30', 'Seafood');
INSERT INTO product_categories (id, name)
VALUES ('fbb342e6-910e-493c-93c7-5b1b58f69cd7', 'Fast Food');
INSERT INTO product_categories (id, name)
VALUES ('fc32339e-b6cc-44e0-80cb-f0e38fc12d6b', 'Pizzas');

CREATE INDEX ix_order_line_items_order_id ON order_line_items (order_id);

CREATE INDEX ix_order_line_items_product_id ON order_line_items (product_id);

CREATE INDEX ix_orders_customer_id ON orders (customer_id);

CREATE UNIQUE INDEX ix_payments_order_id ON payments (order_id);

CREATE UNIQUE INDEX ix_product_categories_name ON product_categories (name);

CREATE INDEX ix_products_category_id ON products (category_id);

CREATE UNIQUE INDEX ix_products_name ON products (name);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250331170854_Initial', '9.0.3');

COMMIT;

