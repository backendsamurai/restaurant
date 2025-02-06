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
    is_verified boolean NOT NULL,
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
VALUES ('1bc7481a-3c30-4505-bf03-f23a12ad1ed9', 'Drinks');
INSERT INTO product_categories (id, name)
VALUES ('4517150a-a5eb-4798-a3dc-8904dbc73edf', 'Desserts');
INSERT INTO product_categories (id, name)
VALUES ('55d15628-f5c8-4540-bf06-5bac43914b14', 'Steaks');
INSERT INTO product_categories (id, name)
VALUES ('a191e59f-16d1-4c91-ac7d-c755085cecaa', 'Sushi');
INSERT INTO product_categories (id, name)
VALUES ('a4563e90-9d5d-41c5-a97f-50a86b5d29b0', 'Fast Food');
INSERT INTO product_categories (id, name)
VALUES ('a9c107b7-8ee8-4446-9ec0-823081a72c69', 'Coffee');
INSERT INTO product_categories (id, name)
VALUES ('baddbe0b-89b6-49ce-b48f-86ee476842d0', 'Pizzas');
INSERT INTO product_categories (id, name)
VALUES ('d3deabee-8072-471f-a693-75eaeb46d272', 'Barbecue');
INSERT INTO product_categories (id, name)
VALUES ('d7f98551-3bb8-4f42-8d04-b520e024f638', 'Seafood');
INSERT INTO product_categories (id, name)
VALUES ('ec46b9b2-472e-49fd-a6e8-4134ced9612a', 'Hot Dogs');

CREATE INDEX ix_order_line_items_order_id ON order_line_items (order_id);

CREATE INDEX ix_order_line_items_product_id ON order_line_items (product_id);

CREATE INDEX ix_orders_customer_id ON orders (customer_id);

CREATE UNIQUE INDEX ix_payments_order_id ON payments (order_id);

CREATE UNIQUE INDEX ix_product_categories_name ON product_categories (name);

CREATE INDEX ix_products_category_id ON products (category_id);

CREATE UNIQUE INDEX ix_products_name ON products (name);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250206145719_Initial', '8.0.8');

COMMIT;

